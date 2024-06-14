using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Exceptions;
using Aspects.Parsers.CSharp.Generated;
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class TypeVisitor : CSharpParserBaseVisitor<TypeInfo>
    {
        public override TypeInfo VisitTuple_type([NotNull] Tuple_typeContext context)
        {
            return new TupleInfo(context.tuple_element()
                .Select(c => new TupleMemberInfo(c.type_().GetText(), c.identifier()?.GetText()))
                .ToArray());
        }

        public override TypeInfo VisitType_declaration([NotNull] Type_declarationContext context)
        {
            var attributeGroups = GetAttributeGroups(context);
            var allModifiers = GetAllMemberModifiers(context);

            if(context.interface_definition() is not null)
                return GetInterface(context.interface_definition(), attributeGroups, allModifiers);
            if (context.class_definition() is not null)
                return GetClass(context.interface_definition(), attributeGroups, allModifiers);
            if (context.enum_definition() is not null)
                return GetEnum(context.enum_definition(), attributeGroups, allModifiers);
            if(context.struct_definition() is not null)
                return GetStruct(context.struct_definition(), attributeGroups, allModifiers);
            if (context.delegate_definition() is not null)
                return GetDelegate(context.delegate_definition(), attributeGroups, allModifiers);

            // TODO make fancy errors
            throw new MalformedCodeException($"malformed type declaration");
        }

        private TypeInfo GetDelegate(Delegate_definitionContext context, IReadOnlyList<AttributeGroupInfo> attributeGroups, IReadOnlyList<string> allModifiers)
        {

        }

        private TypeInfo GetStruct(Struct_definitionContext context, IReadOnlyList<AttributeGroupInfo> attributeGroups, IReadOnlyList<string> allModifiers)
        {

        }

        private TypeInfo GetEnum(Enum_definitionContext context, IReadOnlyList<AttributeGroupInfo> attributeGroups, IReadOnlyList<string> allModifiers)
        {

        }

        private TypeInfo GetClass(Interface_definitionContext context, IReadOnlyList<AttributeGroupInfo> attributeGroups, IReadOnlyList<string> allModifiers)
        {

        }

        private TypeInfo GetInterface(Interface_definitionContext context, IReadOnlyList<AttributeGroupInfo> attributeGroups, IReadOnlyList<string> allModifiers)
        {

        }


        private static IReadOnlyList<string> GetAllMemberModifiers([NotNull] Type_declarationContext context)
        {
            if (context.all_member_modifiers() is null)
                return [];

            return context.all_member_modifiers().all_member_modifier().Select(c => c.GetText()).ToArray();            
        }

        private static IReadOnlyList<AttributeGroupInfo> GetAttributeGroups([NotNull] Type_declarationContext context)
        {
            if (context.attributes() is null)
                return [];

            var sections = new List<AttributeGroupInfo>();
            foreach(var sectionContext in context.attributes().attribute_section())
            {
                var attributeTarget = sectionContext.attribute_target()?.GetText();
                var attributes = new List<AttributeInfo>();

                var attributeContexts = sectionContext.attribute_list()?.attribute();
                if (attributeContexts is not null)
                {
                    foreach(var attributeContext in attributeContexts)
                    {
                        var name = attributeContext.namespace_or_type_name().GetText();
                        var args = new List<ArgumentInfo>();

                        if(attributeContext.attribute_argument() != null)
                        {
                            foreach (var argContext in attributeContext.attribute_argument())
                                args.Add(new ArgumentInfo(argContext.expression().GetText(), argContext.identifier()?.GetText()));
                        }
                        attributes.Add(new AttributeInfo(name, args));
                    }
                }
                sections.Add(new AttributeGroupInfo(attributeTarget, attributes));
            }
            return sections;
        }
    }
}
