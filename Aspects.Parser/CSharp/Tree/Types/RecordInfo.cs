using Aspects.Parsers.CSharp.Tree.Members;

namespace Aspects.Parsers.CSharp.Tree.Types
{
    public class RecordInfo(string name, AccessModifier accessModifier, IReadOnlyList<MemberInfo> members)
        : StructuredTypeInfo(name, accessModifier, members)
    {
        public override TypeKind TypeKind { get; } = TypeKind.Record;
    }
}
