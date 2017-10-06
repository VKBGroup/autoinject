/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject.Locators
{
    public class InjectScoped : InjectAttribute
    {
        public InjectScoped(Type type) : base(type, ServiceLifetime.Scoped)
        {
        }
    }
}