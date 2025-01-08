using Microsoft.CodeAnalysis;
using SourceInjection.CodeAnalysis;
using SourceInjection.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SourceInjection.SourceGeneration.Common
{
    internal class EqualityComparerInfo
    {
        private static readonly Dictionary<string, EqualityComparerInfo> _comparers 
            = new Dictionary<string, EqualityComparerInfo>(64);

        private readonly Lazy<bool> _equalsSupportsNullable;
        private readonly Lazy<bool> _hashCodeSupportsNullable;

        private EqualityComparerInfo(string name, ITypeSymbol type)
        {
            Name = name;
            Type = type;
            _equalsSupportsNullable = new Lazy<bool>(() => EqualsSupportsNullableEvaluation(name, type));
            _hashCodeSupportsNullable = new Lazy<bool>(() => HashCodeSupportsNullableEvaluation(name, type));
        }

        public ITypeSymbol Type { get; }

        public string Name { get; }

        public bool EqualsSupportsNullable => _equalsSupportsNullable.Value;

        public bool HashCodeSupportsNullable => _hashCodeSupportsNullable.Value;

        public static EqualityComparerInfo Get(string name, ITypeSymbol symbol)
        {
            if (string.IsNullOrEmpty(name) || symbol == null)
                return null;

            var key = $"{name}#{symbol.ToDisplayString()}";
            if (_comparers.TryGetValue(key, out var comparer))
                return comparer;

            var c = new EqualityComparerInfo(name, symbol);
            _comparers[key] = c;
            return c;
        }

        private static bool EqualsSupportsNullableEvaluation(string comparerName, ITypeSymbol argType)
        {
            var types = TypeCollector.GetTypes(comparerName);
            if (types.Count > 0)
            {
                var equalsMethod = types.SelectMany(t => t.Symbol.GetAllMembers())
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.IsComparerEqualsMethod(argType));

                return equalsMethod != null
                    && equalsMethod.Parameters.All(p => p.SupportsNullable());
            }

            var type = TypeLoader.GetType(comparerName);
            if (type != null)
            {
                var equalsMethod = Array.Find(type.GetMethods(), m => m.IsComparerEqualsMethod(argType));

                return equalsMethod != null
                    && Array.TrueForAll(equalsMethod.GetParameters(), p => p.SupportsNullable());
            }
            return false;
        }

        private static bool HashCodeSupportsNullableEvaluation(string comparerName, ITypeSymbol argType)
        {
            var types = TypeCollector.GetTypes(comparerName);
            if (types.Count > 0)
            {
                var equalsMethod = types.SelectMany(t => t.Symbol.GetAllMembers())
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => m.IsComparerGetHashCodeMethod(argType));

                return equalsMethod != null
                    && equalsMethod.Parameters[0].SupportsNullable();
            }

            var type = TypeLoader.GetType(comparerName);
            if (type != null)
            {
                var equalsMethod = Array.Find(type.GetMethods(), m => m.IsComparerGetHashCodeMethod(argType));

                return equalsMethod != null
                    && equalsMethod.GetParameters()[0].SupportsNullable();
            }
            return false;
        }
    }
}
