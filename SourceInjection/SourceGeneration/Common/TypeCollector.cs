using System;
using System.Collections.Generic;

namespace SourceInjection.SourceGeneration.Common
{
    internal static class TypeCollector
    {
        private static readonly Dictionary<string, List<TypeInfo>> _allTypes = new Dictionary<string, List<TypeInfo>>(1024);

        public static void Consider(string name, TypeInfo type)
        {
            if (!_allTypes.TryGetValue(name, out var list))
                _allTypes.Add(name, new List<TypeInfo> { type });
            else if (!list.Contains(type))
                list.Add(type);
        }

        public static IReadOnlyList<TypeInfo> GetTypes(string name)
        {
            if (_allTypes.TryGetValue(name, out var value))
                return value;
            return Array.Empty<TypeInfo>();
        }

        public static bool IsRegistered { get; set; }
    }
}
