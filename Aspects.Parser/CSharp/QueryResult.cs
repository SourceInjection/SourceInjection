namespace Aspects.Parsers.CSharp
{
    public class QueryResult
    {
        public IReadOnlyList<Match> Matches { get; }

        public static QueryResult Default => new();
    }
}
