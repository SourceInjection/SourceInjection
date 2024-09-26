using Aspects.Util.SymbolExtensions;
using Microsoft.CodeAnalysis;
using System.Reflection;
using System;
using System.Linq;
using Aspects.Util;

namespace Aspects.SourceGeneration.Common
{
    internal static class EqualityComparerInfo
    {
        public static bool HashCodeSupportsNullable(string comparerName, ITypeSymbol argType)
        {
            var types = TypeInfo.GetTypes(comparerName);
            if (types.Count > 0)
            {
                var equalsMethod = types.SelectMany(t => t.Symbol.GetAllMembers())
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => IsComparerGetHashCodeMethod(m, argType));

                return equalsMethod != null
                    && SupportsNullable(equalsMethod.Parameters[0]);
            }

            var type = Type.GetType(comparerName);
            if (type != null)
            {
                var equalsMethod = Array.Find(type.GetMethods(), m => IsComparerGetHashCodeMethod(m, argType));

                return equalsMethod != null
                    && SupportsNullable(equalsMethod.GetParameters()[0]);
            }
            return false;
        }

        public static bool EqualsSupportsNullable(string comparerName, ITypeSymbol argType)
        {
            var types = TypeInfo.GetTypes(comparerName);
            if (types.Count > 0)
            {
                var equalsMethod = types.SelectMany(t => t.Symbol.GetAllMembers())
                    .OfType<IMethodSymbol>()
                    .FirstOrDefault(m => IsComparerEqualsMethod(m, argType));

                return equalsMethod != null
                    && equalsMethod.Parameters.All(p => SupportsNullable(p));
            }

            var type = Type.GetType(comparerName);
            if (type != null)
            {
                var equalsMethod = Array.Find(type.GetMethods(), m => IsComparerEqualsMethod(m, argType));

                return equalsMethod != null
                    && Array.TrueForAll(equalsMethod.GetParameters(), p => SupportsNullable(p));
            }
            return false;
        }

        private static bool SupportsNullable(ParameterInfo parameter)
        {
            return !parameter.CustomAttributes.Any(a => a.IsDisallowNullAttribute())
                && (Nullable.GetUnderlyingType(parameter.ParameterType) != null || parameter.CustomAttributes.Any(a => a.IsAllowNullAttribute()));
        }

        private static bool SupportsNullable(IParameterSymbol parameter)
        {
            return !parameter.Type.HasDisallowNullAttribute()
                && (parameter.Type.HasNullableAnnotation() || parameter.Type.HasAllowNullAttribute());
        }

        private static bool IsComparerGetHashCodeMethod(IMethodSymbol method, ITypeSymbol argType)
        {
            return method.Name == nameof(GetHashCode)
                && method.ReturnType.IsInt32()
                && method.Parameters.Length == 1
                && argType.Is(method.Parameters[0].Type);
        }

        private static bool IsComparerGetHashCodeMethod(MethodInfo method, ITypeSymbol argType)
        {
            return method.Name == nameof(GetHashCode)
                && method.ReturnType == typeof(int)
                && method.GetParameters().Length == 1
                && IsAssignableFrom(method.GetParameters()[0].ParameterType, argType);
        }

        private static bool IsComparerEqualsMethod(IMethodSymbol method, ITypeSymbol argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType.IsBoolean()
                && method.Parameters.Length == 2
                && method.Parameters.All(p => argType.Is(p.Type));
        }

        private static bool IsComparerEqualsMethod(MethodInfo method, ITypeSymbol argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType == typeof(bool)
                && method.GetParameters().Length == 2
                && Array.TrueForAll(method.GetParameters(), p => IsAssignableFrom(p.ParameterType, argType));
        }

        private static bool IsAssignableFrom(Type type, ITypeSymbol argType)
        {
            var argTypeName = argType.ToDisplayString().TrimEnd('?');
            if(Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            if(Array.Exists(type.GetInterfaces(), itf => itf.FullName == argTypeName))
                return true;

            do
            {
                if (type.FullName == argTypeName)
                    return true;

                type = type.BaseType;
            } while(type != null);

            return false;
        }
    }
}
