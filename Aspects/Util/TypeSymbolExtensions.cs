using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.Util
{
    internal static class TypeSymbolExtensions
    {
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
        /// Checks if the type has a nullable annotation '?'.
        /// </summary>
        /// <param name="symbol">The type to check.</param>
        /// <returns><see cref="true"/> if the type has a '?' annotation </returns>
        public static bool HasNullableAnnotation(this ITypeSymbol symbol)
        {
            return symbol.NullableAnnotation == NullableAnnotation.Annotated;
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
        /// Checks if the <see cref="ITypeSymbol"/> overrides <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="symbol">the <see cref="ITypeSymbol"/> which is checked to override <see cref="object.Equals(object)"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="object.Equals(object)"/> method is overridden else <see langword="false"/>.</returns>
        public static bool OverridesEquals(this ITypeSymbol symbol)
        {
            return symbol.GetMembers().OfType<IMethodSymbol>().Any(
                m => m.Name == nameof(Equals)
                    && m.IsOverride
                    && m.ReturnType.IsBoolean()
                    && m.Parameters.Length == 1
                    && m.Parameters[0].Type.IsObject(true));
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
        /// <param name="symbol">The symbol to be checked.</param>
        /// <param name="allowNullable">Defines if nullable annotatoin '?' is allowed</param>
        /// <returns><see langword="true"/> if the type is primitive else <see langword="false"/></returns>
        public static bool IsPrimitive(this ITypeSymbol symbol, bool allowNullable)
        {
            return symbol.IsBoolean(allowNullable)
                || symbol.IsSByte(allowNullable)
                || symbol.IsByte(allowNullable)
                || symbol.IsInt16(allowNullable)
                || symbol.IsUInt16(allowNullable)
                || symbol.IsInt32(allowNullable)
                || symbol.IsUInt32(allowNullable)
                || symbol.IsInt64(allowNullable)
                || symbol.IsUInt64(allowNullable)
                || symbol.IsSingle(allowNullable)
                || symbol.IsDouble(allowNullable)
                || symbol.IsDecimal(allowNullable)
                || symbol.IsChar(allowNullable)
                || (symbol.IsNativeIntegerType || symbol.TypeKind == TypeKind.Pointer) 
                    && (!symbol.HasNullableAnnotation() || allowNullable);
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
                || symbol.IsString(true)
                || symbol.IsPrimitive(true);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="object"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="object"/> else <see langword="false"/></returns>
        public static bool IsObject(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("object", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Object)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="string"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="string"/> else <see langword="false"/></returns>
        public static bool IsString(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("string", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(String)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="bool"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="bool"/> else <see langword="false"/></returns>
        public static bool IsBoolean(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("bool", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Boolean)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="char"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="char"/> else <see langword="false"/></returns>
        public static bool IsChar(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("char", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Char)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="sbyte"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="sbyte"/> else <see langword="false"/></returns>
        public static bool IsSByte(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("sbyte", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(SByte)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="byte"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="byte"/> else <see langword="false"/></returns>
        public static bool IsByte(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("byte", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Byte)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="short"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="short"/> else <see langword="false"/></returns>
        public static bool IsInt16(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("short", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Int16)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="ushort"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="ushort"/> else <see langword="false"/></returns>
        public static bool IsUInt16(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("ushort", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(UInt16)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="int"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="int"/> else <see langword="false"/></returns>
        public static bool IsInt32(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("int", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Int32)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="uint"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="uint"/> else <see langword="false"/></returns>
        public static bool IsUInt32(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("uint", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(UInt32)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="long"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="long"/> else <see langword="false"/></returns>
        public static bool IsInt64(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("long", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Int64)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="ulong"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="ulong"/> else <see langword="false"/></returns>
        public static bool IsUInt64(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("ulong", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(UInt64)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="float"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="float"/> else <see langword="false"/></returns>
        public static bool IsSingle(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("float", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Single)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="double"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="double"/> else <see langword="false"/></returns>
        public static bool IsDouble(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("double", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Double)}", allowNullable);
        }

        /// <summary>
        /// Checks if the type has exactly type <see langword="decimal"/>.
        /// </summary>
        /// <param name="symbol">The type to be checked.</param>
        /// <param name="allowNullable">Allows nullability annotation '?' if set to <see langword="true"/>.</param>
        /// <returns><see langword="true"/> if the type has exactly type <see langword="decimal"/> else <see langword="false"/></returns>
        public static bool IsDecimal(this ITypeSymbol symbol, bool allowNullable = false)
        {
            return symbol.IsType("decimal", allowNullable)
                || symbol.IsType($"{nameof(System)}.{nameof(Decimal)}", allowNullable);
        }

        private static bool IsType(this ITypeSymbol type, string typeName, bool alsoCheckNullable = false)
        {
            var s = type.ToDisplayString();
            return s == typeName
                || alsoCheckNullable && s == typeName + '?';
        }
    }
}
