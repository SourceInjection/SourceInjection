
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class GenericTypeArguments
    {
        public static List<GenericTypeArgumentDefinition> FromContext(Variant_type_parameter_listContext? context)
        {
            if (context is null)
                return [];

            var args = new List<GenericTypeArgumentDefinition>();
            foreach (var argContext in context.variant_type_parameter())
            {
                args.Add(new GenericTypeArgumentDefinition(
                    argContext.identifier().GetText(),
                    GetVariance(argContext.variance_annotation()),
                    AttributeGroups.FromContext(argContext.attributes())));
            }
            return args;
        }

        public static List<GenericTypeArgumentDefinition> FromContext(Type_parameter_listContext? context)
        {
            if (context is null)
                return [];

            var args = new List<GenericTypeArgumentDefinition>();
            foreach (var argContext in context.type_parameter())
            {
                args.Add(new GenericTypeArgumentDefinition(
                    argContext.identifier().GetText(),
                    null,
                    AttributeGroups.FromContext(argContext.attributes())));
            }
            return args;
        }

        private static Variance? GetVariance(Variance_annotationContext context)
        {
            var s = context?.GetText();
            if (s == "in")
                return Variance.In;
            if (s == "out")
                return Variance.Out;
            return null;
        }
    }
}
