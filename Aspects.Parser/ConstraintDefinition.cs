namespace Aspects.Parsers.CSharp
{
    public class ConstraintDefinition(string targetedTypeArgument, IReadOnlyList<ConstraintClause> clauses )
    {
        public string TargetedTypeArgument => targetedTypeArgument;

        public IReadOnlyList<ConstraintClause> Clauses => clauses;
    }
}
