namespace Aspects.Parsers.CSharp
{
    public class ArgumentInfo(string expression, string? label = null)
    {
        public string Expression => expression;

        public string? Label => label;
    }
}
