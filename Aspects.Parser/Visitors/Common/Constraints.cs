using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Constraints
    {
        public static List<ConstraintDefinition> FromContext(Type_parameter_constraints_clausesContext? context)
        {
            if (context is null)
                return new List<ConstraintDefinition>();

            return context.type_parameter_constraints_clause()
                .Select(c => new ConstraintDefinition(
                        c.identifier().GetText(),
                        GetClauses(c.type_parameter_constraints())))
                .ToList();
        }

        private static List<ConstraintClause> GetClauses(Type_parameter_constraintsContext? context)
        {
            var clauses = new List<ConstraintClause>();
            if (context is null)
                return clauses;

            var primary = GetPrimaryConstraint(context.primary_constraint());
            if (primary is not null)
                clauses.Add(primary);

            clauses.AddRange(GetSecondaryConstraints(context.secondary_constraints()));

            if (context.constructor_constraint() is not null)
                clauses.Add(new ConstraintClause(ConstraintKind.Constructor));

            return clauses;
        }

        private static ConstraintClause? GetPrimaryConstraint(Primary_constraintContext? context)
        {
            if (context is null)
                return null;

            if (context.class_type() is not null)
                return new ConstraintClause(ConstraintKind.OfType, context.class_type().GetText());
            else if (context.STRUCT() is not null)
                return new ConstraintClause(ConstraintKind.Struct);
            else if (context.UNMANAGED() is not null)
                return new ConstraintClause(ConstraintKind.Unmanaged);
            else if(context.NOTNULL() is not null)
                return new ConstraintClause(ConstraintKind.NotNull);
            else if (context.CLASS() is not null)
            {
                if (context.CLASS().GetText().EndsWith('?'))
                    return new ConstraintClause(ConstraintKind.ClassNullable);
                else return new ConstraintClause(ConstraintKind.Class);
            }
            return null;
        }

        private static IEnumerable<ConstraintClause> GetSecondaryConstraints(Secondary_constraintsContext? context)
        {
            if (context is null)
                yield break;

            foreach (var c in context.namespace_or_type_name())
                yield return new ConstraintClause(ConstraintKind.OfType, c.GetText());
        }
    }
}
