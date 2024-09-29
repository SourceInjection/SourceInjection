using Microsoft.CodeAnalysis;
using System;

namespace SourceInjection.Util
{
    internal static class TypeExtensionscs
    {
        public static bool IsAssignableFrom(this Type type, ITypeSymbol argType)
        {
            var argTypeName = argType.ToDisplayString().TrimEnd('?');
            if (Nullable.GetUnderlyingType(type) != null)
                type = Nullable.GetUnderlyingType(type);

            if (Array.Exists(type.GetInterfaces(), itf => itf.FullName == argTypeName))
                return true;

            do
            {
                if (type.FullName == argTypeName)
                    return true;

                type = type.BaseType;
            } while (type != null);

            return false;
        }
    }
}
