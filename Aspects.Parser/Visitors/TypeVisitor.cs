using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Exceptions;
using Aspects.Parsers.CSharp.Generated;
using Aspects.Parsers.CSharp.Visitors.Common;
using static Aspects.Parsers.CSharp.Generated.CSharpParser;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class TypeVisitor : CSharpParserBaseVisitor<TypeDefinition>
    {
        public override TypeDefinition VisitTuple_type([NotNull] Tuple_typeContext context)
        {
            return new TupleDefinition(context.tuple_element()
                .Select(c => new TupleMemberDefinitinon(c.type_().GetText(), c.identifier()?.GetText()))
                .ToArray());
        }

        public override TypeDefinition VisitType_declaration([NotNull] Type_declarationContext context)
        {
            var attributeGroups = AttributeGroups.FromContext(context.attributes());
            var allModifiers = GetAllMemberModifiers(context);

            if(context.interface_definition() is not null)
                return GetInterface(context.interface_definition(), attributeGroups, allModifiers);
            if (context.class_definition() is not null)
                return GetClass(context.interface_definition(), attributeGroups, allModifiers);
            if(context.struct_definition() is not null)
                return GetStruct(context.struct_definition(), attributeGroups, allModifiers);
            if (context.enum_definition() is not null)
                return GetEnum(context.enum_definition(), attributeGroups, allModifiers);
            if (context.delegate_definition() is not null)
                return GetDelegate(context.delegate_definition(), attributeGroups, allModifiers);

            var line = context.Start.Line;
            var col = context.Start.Column;
            throw new MalformedCodeException($"Syntax error at line {line} column {col}: malformed type definition.");
        }

        private static DelegateDefinition GetDelegate(Delegate_definitionContext context, List<AttributeGroup> attributeGroups, string[] allModifiers)
        {
            var modifiers = Modifiers.OfDelegate(allModifiers);
            return new DelegateDefinition(
                name: context.identifier().GetText(),
                accessModifier: modifiers.AccessModifier,
                hasNewModifier: modifiers.HasNewModifier,
                attributeGroups: attributeGroups,
                returnType: context.return_type().GetText(),
                parameters: Parameters.FromContext(context.formal_parameter_list()),
                genericTypeArguments: GenericTypeArguments.FromContext(context.variant_type_parameter_list()),
                constraints: Constraints.FromContext(context.type_parameter_constraints_clauses()));
        }

        private static StructDefinition GetStruct(Struct_definitionContext context, List<AttributeGroup> attributeGroups, string[] allModifiers)
        {
            var modifiers = Modifiers.OfStruct(allModifiers);
            return new StructDefinition(
                name:                 context.identifier().GetText(),
                accessModifier:       modifiers.AccessModifier,
                hasNewModifier:       modifiers.HasNewModifier,
                attributeGroups:      attributeGroups,
                members:              Members.FromContext(context.struct_body()),
                genericTypeArguments: GenericTypeArguments.FromContext(context.type_parameter_list()),
                constraints:          Constraints.FromContext(context.type_parameter_constraints_clauses()),
                isRecord:             modifiers.IsRecord,
                isReadonly:           modifiers.IsReadonly);
        }

        private static EnumDefinition GetEnum(Enum_definitionContext context, List<AttributeGroup> attributeGroups, string[] allModifiers)
        {
            var modifiers = Modifiers.OfEnum(allModifiers);
            return new EnumDefinition(
                name:            context.identifier().GetText(),
                accessModifier:  modifiers.AccessModifier,
                hasNewModifier:  modifiers.HasNewModifier,
                attributeGroups: attributeGroups,
                members:         Members.FromContext(context.enum_body()),
                intType:         context.enum_base()?.type_()?.GetText());
        }

        private static ClassDefinition GetClass(Interface_definitionContext context, List<AttributeGroup> attributeGroups, string[] allModifiers)
        {
            var modifiers = Modifiers.OfClass(allModifiers);
            return new ClassDefinition(
                name:                 context.identifier().GetText(),
                accessModifier:       modifiers.AccessModifier,
                hasNewModifier:       modifiers.HasNewModifier,
                attributeGroups:      attributeGroups,
                members:              Members.FromContext(context.class_body()),
                genericTypeArguments: GenericTypeArguments.FromContext(context.variant_type_parameter_list()),
                constraints:          Constraints.FromContext(context.type_parameter_constraints_clauses()),
                isRecord:             modifiers.IsRecord,
                isStatic:             modifiers.IsStatic,
                isSealed:             modifiers.IsSealed,
                isAbstract:           modifiers.IsAbstract);
        }

        private static InterfaceDefinition GetInterface(Interface_definitionContext context, List<AttributeGroup> attributeGroups, string[] allModifiers)
        {
            var modifiers = Modifiers.OfInterface(allModifiers);
            return new InterfaceDefinition(
                name:                 context.identifier().GetText(),
                accessModifier:       modifiers.AccessModifier,
                hasNewModifier:       modifiers.HasNewModifier,
                attributeGroups:      attributeGroups,
                members:              Members.FromContext(context.class_body()),
                genericTypeArguments: GenericTypeArguments.FromContext(context.variant_type_parameter_list()),
                constraints:          Constraints.FromContext(context.type_parameter_constraints_clauses()));
        }


        private static string[] GetAllMemberModifiers([NotNull] Type_declarationContext context)
        {
            if (context.all_member_modifiers() is null)
                return [];

            return context.all_member_modifiers().all_member_modifier().Select(c => c.GetText()).ToArray();            
        }
    }
}
