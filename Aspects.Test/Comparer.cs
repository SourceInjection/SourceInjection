using System.Reflection;

namespace Aspects.Test
{
    internal static class Comparer
    {
        public static string FromMember(Type containingType, string member)
        {
            var comparerName = containingType.GetMember(member, BindingFlags.Instance | BindingFlags.Public)[0].GetCustomAttributesData()
                .First(attData => attData.ConstructorArguments.Any(ca => ca.ArgumentType == typeof(Type)))!.ConstructorArguments
                .First(ca => ca.ArgumentType == typeof(Type)).ToString();

            if (comparerName.StartsWith("typeof("))
                comparerName = comparerName[7..];
            comparerName = comparerName.TrimEnd(')').Replace('+', '.');

            return comparerName[(comparerName.LastIndexOf('.') + 1)..];
        }
    }
}
