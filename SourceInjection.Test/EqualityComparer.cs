using System.Reflection;

namespace SourceInjection.Test
{
    internal static class EqualityComparer
    {
        public static string FromMember(Type containingType, string member)
        {
            var comparerName = containingType.GetMember(member, BindingFlags.Instance | BindingFlags.Public)[0].GetCustomAttributesData()
                .First(attData => attData.ConstructorArguments.Any(ca => ca.ArgumentType == typeof(Type)))!.ConstructorArguments
                .First(ca => ca.ArgumentType == typeof(Type)).ToString();

            if (comparerName.StartsWith("typeof("))
                comparerName = comparerName[7..];
            comparerName = comparerName.TrimEnd(')');

            if (comparerName.Contains('+'))
                comparerName = comparerName[(comparerName.LastIndexOf('+') + 1)..];

            if (comparerName.Contains('`'))
                return FormatGenericType(comparerName);
            return comparerName;
        }

        private static string FormatGenericType(string comparerName)
        {
            var name = comparerName[..comparerName.IndexOf('`')];
            var genericType = comparerName[(name.Length + 4)..];
            genericType = genericType[..genericType.IndexOf(',')].TrimEnd();
            return $"{name}<{ShortName(genericType)}>";
        }

        private static string ShortName(string name)
        {
            return name switch
            {
                "System.Object" => "object",
                "System.String" => "string",
                "System.Boolean" => "bool",
                "System.Char" => "char",
                "System.Decimal" => "decimal",
                "System.Double" => "double",
                "System.Single" => "float",
                "System.SByte" => "sbyte",
                "System.Int16" => "short",
                "System.Int32" => "int",
                "System.Int64" => "long",
                "System.Byte" => "byte",
                "System.UInt16" => "ushort",
                "System.UInt32" => "uint",
                "System.UInt64" => "ulong",
                _ => name
            };
        }
    }
}
