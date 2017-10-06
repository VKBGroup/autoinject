/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject.Locators
{
    public class EntityContext : InjectAttribute
    {
        public string Connection { get; }
        public string Name { get; }

        public EntityContext(string name = null, string connection = null, ServiceLifetime scope = ServiceLifetime.Scoped) : base(null, scope)
        {
            Connection = connection;
            Name = name;
        }

    }
}
