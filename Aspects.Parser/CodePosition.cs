namespace Aspects.Parsers
{
    public readonly struct CodePosition : IEquatable<CodePosition>, IComparable<CodePosition>
    {
        public int Line { get; }

        public int Column { get; }

        public int CompareTo(CodePosition other)
        {
            var lineRes = Line.CompareTo(other.Line);
            if (lineRes != 0)
                return lineRes;
            return Column.CompareTo(other.Column);
        }

        public bool Equals(CodePosition other)
        {
            return other.Line == Line 
                && other.Column == Column;
        }

        public override bool Equals(object? obj)
        {
            return obj is CodePosition other 
                && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Line, Column);
        }

        public static bool operator ==(CodePosition left, CodePosition right) => left.Equals(right);

        public static bool operator !=(CodePosition left, CodePosition right) => !left.Equals(right);

        public static bool operator <(CodePosition left, CodePosition right) => left.CompareTo(right) < 0;

        public static bool operator <=(CodePosition left, CodePosition right) => left.CompareTo(right) <= 0;

        public static bool operator >(CodePosition left, CodePosition right) => left.CompareTo(right) > 0;

        public static bool operator >=(CodePosition left, CodePosition right) => left.CompareTo(right) >= 0;
    }
}
