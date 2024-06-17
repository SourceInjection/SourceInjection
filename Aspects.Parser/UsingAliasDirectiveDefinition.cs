namespace Aspects.Parsers.CSharp
{
    public class UsingAliasDirectiveDefinition(string value, string newName, string oldName)
        : UsingDirectiveDefinition(value)
    {
        public string NewName => newName;

        public string OldName => oldName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Alias;
    }
}
