﻿using Microsoft.CodeAnalysis;

namespace Aspects.Util
{
    internal static class FieldSymbolExtensions
    {
        public static bool IsInstanceMember(this IFieldSymbol field)
        {
            return !field.IsConst && !field.IsStatic;
        }
    }
}
