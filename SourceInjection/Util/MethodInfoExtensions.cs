using Microsoft.CodeAnalysis;
using System;
using System.Reflection;

namespace SourceInjection.Util
{
    internal static class MethodInfoExtensions
    {
        public static bool IsComparerGetHashCodeMethod(this MethodInfo method, ITypeSymbol argType)
        {
            return method.Name == nameof(GetHashCode)
                && method.ReturnType == typeof(int)
                && method.GetParameters().Length == 1
                && method.GetParameters()[0].ParameterType.IsAssignableFrom(argType);
        }

        public static bool IsComparerEqualsMethod(this MethodInfo method, ITypeSymbol argType)
        {
            return method.Name == nameof(Equals)
                && method.ReturnType == typeof(bool)
                && method.GetParameters().Length == 2
                && Array.TrueForAll(method.GetParameters(), p => p.ParameterType.IsAssignableFrom(argType));
        }
    }
}
