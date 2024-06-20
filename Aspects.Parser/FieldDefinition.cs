namespace Aspects.Parsers.CSharp
{
    public class FieldDefinition: MemberDefinition
    {
        public FieldDefinition(string name, AccessModifier accessModifier, bool hasNewModifier,
            string type, bool isStatic, bool isReadonly, bool isNew)

            : base(name, accessModifier, hasNewModifier)
        {
            Type = type;
            IsStatic = isStatic;
            IsReadonly = isReadonly;
            IsNew = isNew;
        }

        public override MemberKind MemberKind { get; } = MemberKind.Field;

        public override AccessModifier DefaultAccessability { get; } = CSharp.AccessModifier.Private;

        public string Type { get; }

        public bool IsStatic { get; }

        public bool IsReadonly { get; }

        public bool IsNew { get; }
    }
}
