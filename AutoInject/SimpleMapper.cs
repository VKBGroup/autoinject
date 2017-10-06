/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using com.VKB.AutoInject.Locators;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject
{
    internal class SimpleMapper<T> where T : InjectAttribute
    {
        protected readonly IServiceCollection _serviceCollection;
#if DEBUG
        public Action<string> Log { get; set; }
#endif

        internal SimpleMapper(IServiceCollection services)
        {
            _serviceCollection = services;
        }

        internal void FindAndMap()
        {
            IEnumerable<Type> concreteImpls = FindConcereteTypes();
#if DEBUG
            Log($"Found [{concreteImpls.Count()}] AutoInject service mapping targets.");
#endif
            MapConcreteTypesToServiceInterfaces(concreteImpls);
        }

        protected IEnumerable<Type> FindConcereteTypes()
        {
            return AssemblyUtils.FindTagged<T>();
        }

        protected void MapConcreteTypesToServiceInterfaces(IEnumerable<Type> concreteImplTypes)
        {
            foreach (Type concreteImpl in concreteImplTypes)
            {
                var injectAttribute = concreteImpl.GetCustomAttribute<T>();
                MapToService(injectAttribute, concreteImpl);
            }
        }

        protected virtual void MapToService(T injectAttribute, Type concreteImplType)
        {
            if (injectAttribute.InjectTargetInterface == null) return;

            ServiceDescriptor sd = new ServiceDescriptor(injectAttribute.InjectTargetInterface, concreteImplType, injectAttribute.Scope);
            _serviceCollection.Add(sd);
#if DEBUG
            Log($"Mapped ({injectAttribute.InjectTargetInterface.FullName}) to ({concreteImplType.FullName} as [{injectAttribute.Scope}])");
#endif
        }
    }
}