namespace Aspects.Parsers.CSharp
{
    public class FieldDefinition(string name, AccessModifier accessModifier, bool hasNewModifier,
        string type, bool isStatic, bool isReadonly, bool isNew)

        : MemberDefinition(name, accessModifier, hasNewModifier)
    {
        public override MemberKind MemberKind { get; } = MemberKind.Field;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Type => type;

        public bool IsStatic => isStatic;

        public bool IsReadonly => isReadonly;

        public bool IsNew => isNew;
    }
}
