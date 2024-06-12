namespace Aspects.Parsers.Tree
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    internal class UsingStaticDirective(string value, string classFullName)
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        : UsingDirective(value)
    {
        public string ClassFullName => classFullName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Static;

        public override bool Equals(object? obj)
        {
            return obj == this || obj is UsingStaticDirective other
                && base.Equals(obj)
                && Kind == other.Kind
                && ClassFullName == other.ClassFullName;
        }
    }
}
