using Aspects.Parsers.CSharp.Tree.Members;

namespace Aspects.Parsers.CSharp.Tree.Types
{
    internal class EnumInfo(string name, AccessModifier accessModifier, IReadOnlyList<EnumMemberInfo> members, 
        string? intType = null) 

        : TypeInfo(name, accessModifier, members)
    {
        public string IntType { get; } = GetIntType(intType);

        public new IReadOnlyList<EnumMemberInfo> Members => members;

        public override TypeKind TypeKind { get; } = TypeKind.Enum;

        private static string GetIntType(string? intType)
        {
            return intType ?? "int";
        }
    }
}
