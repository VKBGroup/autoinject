/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject.Locators
{
    public class InjectSingleton : InjectAttribute
    {
        public InjectSingleton(Type type) : base(type, ServiceLifetime.Singleton)
        {
        }
    }
}