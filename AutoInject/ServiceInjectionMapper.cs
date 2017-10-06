/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using System.Diagnostics;
using com.VKB.AutoInject.Locators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject
{
    public static class ServiceInjectionMapper
    {
#if DEBUG
        private static readonly Action<string> LogWriteAction = s => Debug.WriteLine(s);
#endif
        public static void AutoInject(this IServiceCollection services, IConfiguration configuration = null)
        {
            var generic = new SimpleMapper<InjectAttribute>(services);
            var dbContextMapper = new DbContextMapper(services, configuration);
#if DEBUG
            generic.Log = LogWriteAction;
            dbContextMapper.Log = LogWriteAction;
#endif
            generic.FindAndMap();
            dbContextMapper.FindAndMap();
        }
    }
}