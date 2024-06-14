namespace Aspects.Parsers.CSharp
{
    public class FieldInfo(string name, AccessModifier accessModifier,
        string type, bool isStatic, bool isReadonly, bool isNew)

        : MemberInfo(name, accessModifier)
    {
        public override MemberKind MemberKind { get; } = MemberKind.Field;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Type => type;

        public bool IsStatic => isStatic;

        public bool IsReadonly => isReadonly;

        public bool IsNew => isNew;
    }
}
