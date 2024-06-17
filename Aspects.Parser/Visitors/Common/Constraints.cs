using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Constraints
    {
        public static List<ConstraintDefinition> FromContext(Type_parameter_constraints_clausesContext? context)
        {
            if (context is null)
                return [];

            var members = new List<ConstraintDefinition>();
            // TODO
            return members;
        }
    }
}
