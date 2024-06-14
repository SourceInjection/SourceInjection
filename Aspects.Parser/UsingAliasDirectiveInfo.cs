namespace Aspects.Parsers.CSharp
{
    public class UsingAliasDirectiveInfo(string value, string newName, string oldName)
        : UsingDirectiveInfo(value)
    {
        public string NewName => newName;

        public string OldName => oldName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Alias;
    }
}
