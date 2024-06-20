namespace Aspects.Parsers.CSharp
{
    public class UsingAliasDirectiveDefinition : UsingDirectiveDefinition
    {
        public UsingAliasDirectiveDefinition(string value, string newName, string oldName)
            : base(value)
        {
            NewName = newName;
            OldName = oldName;
        }

        public string NewName { get; }

        public string OldName { get; }

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Alias;
    }
}
