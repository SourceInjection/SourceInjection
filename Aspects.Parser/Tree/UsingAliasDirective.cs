namespace Aspects.Parsers.Tree
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    internal class UsingAliasDirective(string value, string newName, string oldName, bool isTypeDefinition)
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        : UsingDirective(value)
    {
        public string NewName => newName;

        public string OldName => oldName;

        public bool IsTypeDefinition => isTypeDefinition;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Alias;

        public override bool Equals(object? obj)
        {
            return obj == this || obj is UsingAliasDirective other
                && base.Equals(obj)
                && NewName == other.NewName
                && OldName == other.OldName
                && IsTypeDefinition == other.IsTypeDefinition;
        }
    }
}
