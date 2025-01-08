using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SourceInjection.SourceGeneration.Common
{
    internal static class TypeLoader
    {
        public static Type GetType(string name, bool throwIfNotFond = false)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.GetType(name) is Type t)
                    return t;
            }

            if (throwIfNotFond)
            {
                var assembliesString = string.Join(", ", assemblies.Select(a => a.GetName().Name)); 
                throw new TypeLoadException($"Type '{name}' not found in assemblies {assembliesString}");
            }
            return null;
        }
    }
}
