/*
 * Author : Gerhard Davids [glacieredpyro@gmail.com]
 * Copyright : VKB 2017
 * License : See LICENSE.MD
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

namespace com.VKB.AutoInject
{
    class AssemblyUtils
    {
        static IEnumerable<Assembly> GetReferencingAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;

            foreach (var library in dependencies)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
                catch (FileNotFoundException)
                { }
            }
            //exclude MS assemblies because they seem to break if they arent found at compile time.
            //Havent figured out why yet
            return assemblies.Where(a => !a.FullName.StartsWith("Microsoft"));
        }

        internal static IEnumerable<Type> FindTagged<T>() where T : Attribute
        {
            IEnumerable<Type> tagged = from a in GetReferencingAssemblies()
                from t in a.GetTypes()
                where t.GetCustomAttribute<T>() != null
                select t;

            return tagged;
        }
    }
}
