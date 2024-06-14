namespace Aspects.Parsers.CSharp
{
    public class UsingAliasDirective(string value, string newName, string oldName, bool isTypeDefinition)
        : UsingDirective(value)
    {
        public string NewName => newName;

        public string OldName => oldName;

        public bool IsTypeDefinition => isTypeDefinition;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Alias;
    }
}
