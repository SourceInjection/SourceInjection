namespace Aspects.Parsers.CSharp.Tree.Usings
{
    public class UsingStaticDirective(string value, string classFullName)
        : UsingDirective(value)
    {
        public string ClassFullName => classFullName;

        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.Static;
    }
}
