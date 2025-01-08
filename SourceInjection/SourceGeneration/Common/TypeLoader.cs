using System;
using System.Collections.Generic;
using System.Reflection;

namespace SourceInjection.SourceGeneration.Common
{
    internal static class TypeLoader
    {
        private static Assembly[] _assemblies = null;
        private static readonly Dictionary<string, Type> _loadedTypes = new Dictionary<string, Type>(1024);

        private static Assembly[] Assemblies
        {
            get
            {
                if (_assemblies == null)
                    _assemblies = AppDomain.CurrentDomain.GetAssemblies();
                return _assemblies;
            }
        }

        public static Type GetType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (_loadedTypes.TryGetValue(name, out Type type))
                return type;

            type = null;
            foreach (var assembly in Assemblies)
            {
                if (assembly.GetType(name) is Type t)
                {
                    type = t;
                    break;
                }
            }

            _loadedTypes[name] = type;
            return type;
        }
    }
}
