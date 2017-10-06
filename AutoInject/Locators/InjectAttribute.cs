/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using Microsoft.Extensions.DependencyInjection;

namespace com.VKB.AutoInject.Locators
{
    public class InjectAttribute : Attribute
    {

        public ServiceLifetime Scope { get; }
        public Type InjectTargetInterface { get; }
     
        public InjectAttribute(Type type, ServiceLifetime scope = ServiceLifetime.Transient)
        {
            InjectTargetInterface = type;
            Scope = scope;
        }
    }
}
