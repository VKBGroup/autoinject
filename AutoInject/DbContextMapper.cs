/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using System.Reflection;
using com.VKB.AutoInject.Locators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject
{
    internal class DbContextMapper : SimpleMapper<EntityContext>
    {
        private readonly IConfiguration _configuration;

        internal DbContextMapper(IServiceCollection services, IConfiguration configuration) : base(services)
        {
            _configuration = configuration;
        }

        protected override void MapToService(EntityContext injectAttribute, Type concreteImplType)
        {
            try
            {
                AddDbContext(concreteImplType, CreateContextOptions(injectAttribute), injectAttribute);
#if DEBUG
                Log($"Mapped ({injectAttribute.TypeId}) to ({concreteImplType.FullName} as [{injectAttribute.Scope}])");
#endif
            }
            catch (NullReferenceException)
            {
                throw new NoConfigurationSpecifiedException(concreteImplType, injectAttribute);
            }
        }

        private void AddDbContext(Type dbContextType, Action<DbContextOptionsBuilder> optionsBuilder, EntityContext injectAttribute)
        {
            //basically services.AddDbContext<DbContext>() with dynamic type argument
            MethodInfo meth = typeof(EntityFrameworkServiceCollectionExtensions)
                .GetMethod("AddDbContext",
                    new Type[]
                    {
                        typeof(IServiceCollection),
                        typeof(Action<DbContextOptionsBuilder>),
                        typeof(ServiceLifetime),
                        typeof(ServiceLifetime)
                    })
                .MakeGenericMethod(dbContextType);
            meth.Invoke(null, new object[] { _serviceCollection, optionsBuilder ?? Type.Missing, injectAttribute.Scope, Type.Missing });
        }

        private Action<DbContextOptionsBuilder> CreateContextOptions(EntityContext entityContext)
        {
            var conn = GetConnectionString(entityContext);
            if (conn == null) return null;

            return optionsBuilder => optionsBuilder.UseSqlServer(conn);
        }

        private string GetConnectionString(EntityContext entityContext)
        {
            if (entityContext.Name != null)
            {
                if(_configuration == null) throw new NullReferenceException();
                return _configuration.GetSection("ConnectionStrings")?[entityContext.Name];
            }
            return entityContext.Connection;
        }
    }

    internal class NoConfigurationSpecifiedException : Exception
    {
        public NoConfigurationSpecifiedException(Type concreteImpl, EntityContext context)
            :base($"{concreteImpl.FullName} specified with Connection[\"{context.Name}\"] but no IConfiguration provided to AutoInject()")
        {
                
        }
    }
}