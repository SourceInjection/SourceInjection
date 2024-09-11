using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System;
using System.Linq;
using Aspects.Util;

namespace Aspects.SourceGeneration.Common
{
    internal class EqualityComparerInfo
    {
        public EqualityComparerInfo(string comparerName, ITypeSymbol typeToCompare)
        {
            EqualsSupportsNullable = ComparerEqualsSupportsNullable(comparerName, typeToCompare);
            HashCodeSupportsNullable = false;
        }


        public bool EqualsSupportsNullable { get; }

        public bool HashCodeSupportsNullable { get; }


        private static bool ComparerEqualsSupportsNullable(string comparerName, ITypeSymbol argType)
        {
            var types = TypeInfo.Get(comparerName);
            if (types.Count > 0)
            {
                var equalsMethod = types.SelectMany(t => t.Symbol.GetAllMembers())
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => IsComparerEqualsMethod(m, argType));

                return equalsMethod != null
                    && equalsMethod.Parameters.All(p => p.Type.HasNullableAnnotation() || p.Type.HasMaybeNullAttribute());
            }

            var type = Type.GetType(comparerName);
            if (type != null)
            {
                var argT = Type.GetType(argType.ToDisplayString().TrimEnd('?'));
                var equalsMethod = type.GetMethods()
                    .FirstOrDefault(m => IsComparerEqualsMethod(m, argT));

                return equalsMethod != null
                    && equalsMethod.GetParameters().All(p => Nullable.GetUnderlyingType(p.ParameterType) != null);
            }
            return false;
        }

        private static bool IsComparerEqualsMethod(IMethodSymbol method, ITypeSymbol argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType.IsBoolean()
                && method.Parameters.Length == 2
                && method.Parameters.All(p => argType.Is(p.Type));
        }

        private static bool IsComparerEqualsMethod(MethodInfo method, Type argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType == typeof(bool)
                && method.GetParameters().Length == 2
                && method.GetParameters().All(p => p.ParameterType.IsAssignableFrom(argType));
        }
    }
}
