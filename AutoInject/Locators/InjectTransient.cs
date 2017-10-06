/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;

namespace com.VKB.AutoInject.Locators
{
    public class InjectTransient : InjectAttribute
    {
        public InjectTransient(Type type) : base(type)
        {

        }
    }
}
