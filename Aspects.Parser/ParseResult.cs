
using Aspects.Parsers.Util;

namespace Aspects.Parsers
{
    internal class ParseResult<T>(T? value, IReadOnlyList<string> syntaxErrors)
    {
        private int? _hashCode;

        public IReadOnlyList<string> SyntaxErrors => syntaxErrors;

        public T? Value { get; } = value;

        public override bool Equals(object? obj)
        {
            return obj is ParseResult<T> other
                && Value?.Equals(other.Value) is true
                && SyntaxErrors.SequenceEqual(other.SyntaxErrors);
        }

        public override int GetHashCode()
        {
            _hashCode ??= HashCode.Combine(Value, SyntaxErrors.CombinedHashCode());
            return _hashCode.Value;
        }
    }
}
