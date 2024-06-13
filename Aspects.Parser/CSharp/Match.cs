namespace Aspects.Parsers.CSharp
{
    public class Match(CodePosition position, string value)
    {
        public CodePosition Position => position;

        public string Value => value;

        public override bool Equals(object? obj)
        {
            return obj is Match match 
                && Position == match.Position 
                && Value == match.Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Position, Value);
        }
    }
}
