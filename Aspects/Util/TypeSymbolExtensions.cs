using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class TypeSymbolExtensions
    {
        private static readonly string[] SimpleTypes = new string[]
        {
            nameof(Byte), nameof(SByte), nameof(Int16), nameof(UInt16), 
            nameof(Int32), nameof(UInt32), nameof(Int64), nameof(UInt64),
            nameof(Boolean), nameof(Char),
            nameof(Single), nameof(Double), nameof(Decimal)
        };

        /// <summary>
        /// Computes the inheritance of a <see cref="ITypeSymbol"/> (including itself).
        /// </summary>
        /// <param name="symbol">The <see cref="ITypeSymbol"/> for which the inheritance is computed.</param>
        /// <returns>A <see cref="IEnumerable"/> of <see cref="ITypeSymbol"/>s representing the inheritance from top to bottom.</returns>
        public static IEnumerable<ITypeSymbol> Inheritance(this ITypeSymbol symbol)
        {
            while (symbol != null)
            {
                yield return symbol;
                symbol = symbol.BaseType;
            }
        }

        /// <summary>
        /// Checks if the symbol implements the given interface.
        /// </summary>
        /// <typeparam name="T">The interface type to be checked.</typeparam>
        /// <param name="symbol">The symbol for wich the interface implementation is checked.</param>
        /// <returns><see langword="true"/> if the symbol implements the given interface else <see langword="false"/></returns>
        public static bool Implements<T>(this ITypeSymbol symbol)
        {
            var name = typeof(T).FullName;
            return symbol.AllInterfaces.Any(i => i.ToDisplayString() == name);
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> implements the interface <see cref="IEnumerable"/>.
        /// </summary>
        /// <param name="symbol">The <see cref="ITypeSymbol"/> which is checked to be a <see cref="IEnumerable"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="ITypeSymbol"/> is a <see cref="IEnumerable"/> else <see langword="false"/></returns>
        public static bool IsEnumerable(this ITypeSymbol symbol)
        {
            return symbol.ToDisplayString() == CodeSnippets.IEnumerableName
                || symbol.AllInterfaces.Any(i => i.ToDisplayString() == CodeSnippets.IEnumerableName);
        }


        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> overrides <see cref="object.Equals(object)"/> itself or in any base class.
        /// </summary>
        /// <param name="symbol">the <see cref="ITypeSymbol"/> which is checked to override <see cref="object.Equals(object)"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="object.Equals(object)"/> method is overridden else <see langword="false"/>.</returns>
        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(Equals)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "bool"
                && m.Parameters.Length == 1
                && m.Parameters[0].Type.IsTypeOrNullableType("object")));
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> overrides <see cref="object.GetHashCode"/> itself or in any base class.
        /// </summary>
        /// <param name="symbol">the <see cref="ITypeSymbol"/> which is checked to override <see cref="object.GetHashCode"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="object.GetHashCode"/> method is overridden else <see langword="false"/></returns>
        public static bool OverridesGetHashCode(this ITypeSymbol symbol)
        {
            return symbol.Inheritance().Any(cl => cl.GetMembers().OfType<IMethodSymbol>().Any(m =>
                m.Name == nameof(GetHashCode)
                && m.IsOverride
                && m.ReturnType.ToDisplayString() == "int"));
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> is a primitive type.<br/>
        /// The following types count as primitive:<br/>
        /// <see langword="sbyte"/>, <see langword="byte"/>, 
        /// <see langword="short"/>, <see langword="ushort"/>, 
        /// <see langword="int"/>, <see langword="uint"/>, 
        /// <see langword="long"/>, <see langword="ulong"/>,
        /// <see langword="nint"/>, <see langword="nuint"/>,
        /// <see langword="char"/>, <see langword="bool"/>,
        /// <see langword="float"/>, <see langword="double"/>, <see langword="decimal"/>.<br/>
        /// Also any pointer type counts as primitive type.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns><see langword="true"/> if the type is primitive else <see langword="false"/></returns>
        public static bool IsPrimitive(this ITypeSymbol symbol)
        {
            return symbol.IsUnmanagedType
                && !symbol.IsTupleType
                && symbol.TypeKind != TypeKind.Enum
                && symbol.TypeKind != TypeKind.Struct
                || symbol.IsNativeIntegerType
                || SimpleTypes.Contains(symbol.Name);
            // TODO check if it is a struct or a primitive
        }

        /// <summary>
        /// Checks if the <see cref="ITypeSymbol"/> is a <see langword="string"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns><see langword="true"/> if the type is a <see langword="string"/> else <see langword="false"/></returns>
        public static bool IsString(this ITypeSymbol symbol)
        {
            return symbol.Name == nameof(String);
        }

        /// <summary>
        /// Checks if the type can be compared by default with equality comparison operators '==' and '!='.<br/>
        /// Supported are <see langword="record"/>s, <see langword="enum"/>s, <see langword="string"/>s, 
        /// and primitive types (<see cref="IsPrimitive(ITypeSymbol)"/>).
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns><see langword="true"/> if the type can use equality operators by default else <see langword="false"/></returns>
        public static bool CanUseEqualityOperatorsByDefault(this ITypeSymbol symbol)
        {
            return symbol.IsRecord
                || symbol.TypeKind == TypeKind.Enum
                || symbol.IsString()
                || symbol.IsPrimitive();
        }

        private static bool IsTypeOrNullableType(this ITypeSymbol symbol, string typeName)
        {
            var s = symbol.ToDisplayString();
            return s == typeName
                || s == typeName + '?';
        }
    }
}
