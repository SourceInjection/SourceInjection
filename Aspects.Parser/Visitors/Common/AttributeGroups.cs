using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class AttributeGroups
    {
        public static List<AttributeGroup> FromContext(AttributesContext? context)
        {
            if (context?.attribute_section() is null)
                return [];

            var sections = new List<AttributeGroup>();
            foreach (var sectionContext in context.attribute_section())
            {
                var attributeTarget = sectionContext.attribute_target()?.GetText();
                var attributes = new List<AttributeUsage>();

                var attributeContexts = sectionContext.attribute_list()?.attribute();
                if (attributeContexts is not null)
                {
                    foreach (var attributeContext in attributeContexts)
                    {
                        var name = attributeContext.namespace_or_type_name().GetText();
                        var args = new List<Argument>();

                        if (attributeContext.attribute_argument() != null)
                        {
                            foreach (var argContext in attributeContext.attribute_argument())
                                args.Add(new Argument(argContext.expression().GetText(), argContext.identifier()?.GetText()));
                        }
                        attributes.Add(new AttributeUsage(name, args));
                    }
                }
                sections.Add(new AttributeGroup(attributeTarget, attributes));
            }
            return sections;
        }
    }
}
