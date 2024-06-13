namespace Aspects.Parsers.CSharp.Tree
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    internal class UsingNamespaceDirective(string value, string nameSpace)
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        : UsingDirective(value)
    {
        public string Namespace => nameSpace;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Namespace;

        public override bool Equals(object? obj)
        {
            return obj == this || obj is UsingNamespaceDirective other
                && base.Equals(obj)
                && Kind == other.Kind
                && Namespace == other.Namespace;
        }
    }
}
