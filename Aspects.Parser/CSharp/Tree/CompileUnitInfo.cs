namespace Aspects.Parsers.CSharp.Tree
{
    internal class CompileUnitInfo
    {
        private string? _asString;

        public IReadOnlyList<UsingDirective> Directives { get; }

        public IReadOnlyList<TypeInfo> Types { get; }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override string ToString()
        {
            _asString ??= AsString();
            return _asString;
        }

        private string AsString()
        {

        }

    }
}
