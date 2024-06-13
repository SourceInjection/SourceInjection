
namespace Aspects.Parsers.CSharp.Tree.Members
{
    public class FieldInfo(string name, AccessModifier accessModifier, 
        string type, bool isStatic, bool isReadonly, bool isNew)

        : MemberInfo(name, accessModifier)
    {
        public override MemberKind MemberKind { get; } = MemberKind.Field;

        public string Type => type;

        public bool IsStatic => isStatic;

        public bool IsReadonly => isReadonly;

        public bool IsNew => isNew;
    }
}
