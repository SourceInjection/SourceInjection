
using Antlr4.Runtime.Misc;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Visitors
{
    internal class QueryVisitor : CSharpParserBaseVisitor<QueryResult>
    {
        protected override QueryResult DefaultResult => QueryResult.Default;

        public override QueryResult VisitAccessor_body([NotNull] CSharpParser.Accessor_bodyContext context)
        {
            return base.VisitAccessor_body(context);
        }

        public override QueryResult VisitAccessor_declarations([NotNull] CSharpParser.Accessor_declarationsContext context)
        {
            return base.VisitAccessor_declarations(context);
        }

        public override QueryResult VisitAccessor_modifier([NotNull] CSharpParser.Accessor_modifierContext context)
        {
            return base.VisitAccessor_modifier(context);
        }

        public override QueryResult VisitAdditive_expression([NotNull] CSharpParser.Additive_expressionContext context)
        {
            return base.VisitAdditive_expression(context);
        }

        public override QueryResult VisitAdd_accessor_declaration([NotNull] CSharpParser.Add_accessor_declarationContext context)
        {
            return base.VisitAdd_accessor_declaration(context);
        }

        public override QueryResult VisitAll_member_modifier([NotNull] CSharpParser.All_member_modifierContext context)
        {
            return base.VisitAll_member_modifier(context);
        }

        public override QueryResult VisitAll_member_modifiers([NotNull] CSharpParser.All_member_modifiersContext context)
        {
            return base.VisitAll_member_modifiers(context);
        }

        public override QueryResult VisitAnd_expression([NotNull] CSharpParser.And_expressionContext context)
        {
            return base.VisitAnd_expression(context);
        }

        public override QueryResult VisitAnonymousMethodExpression([NotNull] CSharpParser.AnonymousMethodExpressionContext context)
        {
            return base.VisitAnonymousMethodExpression(context);
        }

        public override QueryResult VisitAnonymous_function_body([NotNull] CSharpParser.Anonymous_function_bodyContext context)
        {
            return base.VisitAnonymous_function_body(context);
        }

        public override QueryResult VisitAnonymous_function_signature([NotNull] CSharpParser.Anonymous_function_signatureContext context)
        {
            return base.VisitAnonymous_function_signature(context);
        }

        public override QueryResult VisitAnonymous_object_initializer([NotNull] CSharpParser.Anonymous_object_initializerContext context)
        {
            return base.VisitAnonymous_object_initializer(context);
        }

        public override QueryResult VisitArgument([NotNull] CSharpParser.ArgumentContext context)
        {
            return base.VisitArgument(context);
        }

        public override QueryResult VisitArgument_list([NotNull] CSharpParser.Argument_listContext context)
        {
            return base.VisitArgument_list(context);
        }

        public override QueryResult VisitArg_declaration([NotNull] CSharpParser.Arg_declarationContext context)
        {
            return base.VisitArg_declaration(context);
        }

        public override QueryResult VisitArray_initializer([NotNull] CSharpParser.Array_initializerContext context)
        {
            return base.VisitArray_initializer(context);
        }

        public override QueryResult VisitArray_type([NotNull] CSharpParser.Array_typeContext context)
        {
            return base.VisitArray_type(context);
        }

        public override QueryResult VisitAssignment([NotNull] CSharpParser.AssignmentContext context)
        {
            return base.VisitAssignment(context);
        }

        public override QueryResult VisitAssignment_operator([NotNull] CSharpParser.Assignment_operatorContext context)
        {
            return base.VisitAssignment_operator(context);
        }

        public override QueryResult VisitAttribute([NotNull] CSharpParser.AttributeContext context)
        {
            return base.VisitAttribute(context);
        }

        public override QueryResult VisitAttributes([NotNull] CSharpParser.AttributesContext context)
        {
            return base.VisitAttributes(context);
        }

        public override QueryResult VisitAttribute_argument([NotNull] CSharpParser.Attribute_argumentContext context)
        {
            return base.VisitAttribute_argument(context);
        }

        public override QueryResult VisitAttribute_list([NotNull] CSharpParser.Attribute_listContext context)
        {
            return base.VisitAttribute_list(context);
        }

        public override QueryResult VisitAttribute_section([NotNull] CSharpParser.Attribute_sectionContext context)
        {
            return base.VisitAttribute_section(context);
        }

        public override QueryResult VisitAttribute_target([NotNull] CSharpParser.Attribute_targetContext context)
        {
            return base.VisitAttribute_target(context);
        }

        public override QueryResult VisitBaseAccessExpression([NotNull] CSharpParser.BaseAccessExpressionContext context)
        {
            return base.VisitBaseAccessExpression(context);
        }

        public override QueryResult VisitBase_type([NotNull] CSharpParser.Base_typeContext context)
        {
            return base.VisitBase_type(context);
        }

        public override QueryResult VisitBlock([NotNull] CSharpParser.BlockContext context)
        {
            return base.VisitBlock(context);
        }

        public override QueryResult VisitBody([NotNull] CSharpParser.BodyContext context)
        {
            return base.VisitBody(context);
        }

        public override QueryResult VisitBoolean_literal([NotNull] CSharpParser.Boolean_literalContext context)
        {
            return base.VisitBoolean_literal(context);
        }

        public override QueryResult VisitBracket_expression([NotNull] CSharpParser.Bracket_expressionContext context)
        {
            return base.VisitBracket_expression(context);
        }

        public override QueryResult VisitBreakStatement([NotNull] CSharpParser.BreakStatementContext context)
        {
            return base.VisitBreakStatement(context);
        }

        public override QueryResult VisitCase_guard([NotNull] CSharpParser.Case_guardContext context)
        {
            return base.VisitCase_guard(context);
        }

        public override QueryResult VisitCast_expression([NotNull] CSharpParser.Cast_expressionContext context)
        {
            return base.VisitCast_expression(context);
        }

        public override QueryResult VisitCatch_clauses([NotNull] CSharpParser.Catch_clausesContext context)
        {
            return base.VisitCatch_clauses(context);
        }

        public override QueryResult VisitCheckedExpression([NotNull] CSharpParser.CheckedExpressionContext context)
        {
            return base.VisitCheckedExpression(context);
        }

        public override QueryResult VisitCheckedStatement([NotNull] CSharpParser.CheckedStatementContext context)
        {
            return base.VisitCheckedStatement(context);
        }

        public override QueryResult VisitClass_base([NotNull] CSharpParser.Class_baseContext context)
        {
            return base.VisitClass_base(context);
        }

        public override QueryResult VisitClass_body([NotNull] CSharpParser.Class_bodyContext context)
        {
            return base.VisitClass_body(context);
        }

        public override QueryResult VisitClass_definition([NotNull] CSharpParser.Class_definitionContext context)
        {
            return base.VisitClass_definition(context);
        }

        public override QueryResult VisitClass_member_declaration([NotNull] CSharpParser.Class_member_declarationContext context)
        {
            return base.VisitClass_member_declaration(context);
        }

        public override QueryResult VisitClass_member_declarations([NotNull] CSharpParser.Class_member_declarationsContext context)
        {
            return base.VisitClass_member_declarations(context);
        }

        public override QueryResult VisitClass_type([NotNull] CSharpParser.Class_typeContext context)
        {
            return base.VisitClass_type(context);
        }

        public override QueryResult VisitCollection_initializer([NotNull] CSharpParser.Collection_initializerContext context)
        {
            return base.VisitCollection_initializer(context);
        }

        public override QueryResult VisitCombined_join_clause([NotNull] CSharpParser.Combined_join_clauseContext context)
        {
            return base.VisitCombined_join_clause(context);
        }

        public override QueryResult VisitCommon_member_declaration([NotNull] CSharpParser.Common_member_declarationContext context)
        {
            return base.VisitCommon_member_declaration(context);
        }

        public override QueryResult VisitCompilation_unit([NotNull] CSharpParser.Compilation_unitContext context)
        {
            return base.VisitCompilation_unit(context);
        }

        public override QueryResult VisitConditional_and_expression([NotNull] CSharpParser.Conditional_and_expressionContext context)
        {
            return base.VisitConditional_and_expression(context);
        }

        public override QueryResult VisitConditional_expression([NotNull] CSharpParser.Conditional_expressionContext context)
        {
            return base.VisitConditional_expression(context);
        }

        public override QueryResult VisitConditional_or_expression([NotNull] CSharpParser.Conditional_or_expressionContext context)
        {
            return base.VisitConditional_or_expression(context);
        }

        public override QueryResult VisitConstant_declaration([NotNull] CSharpParser.Constant_declarationContext context)
        {
            return base.VisitConstant_declaration(context);
        }

        public override QueryResult VisitConstant_declarator([NotNull] CSharpParser.Constant_declaratorContext context)
        {
            return base.VisitConstant_declarator(context);
        }

        public override QueryResult VisitConstant_declarators([NotNull] CSharpParser.Constant_declaratorsContext context)
        {
            return base.VisitConstant_declarators(context);
        }

        public override QueryResult VisitConstructor_constraint([NotNull] CSharpParser.Constructor_constraintContext context)
        {
            return base.VisitConstructor_constraint(context);
        }

        public override QueryResult VisitConstructor_declaration([NotNull] CSharpParser.Constructor_declarationContext context)
        {
            return base.VisitConstructor_declaration(context);
        }

        public override QueryResult VisitConstructor_initializer([NotNull] CSharpParser.Constructor_initializerContext context)
        {
            return base.VisitConstructor_initializer(context);
        }

        public override QueryResult VisitContinueStatement([NotNull] CSharpParser.ContinueStatementContext context)
        {
            return base.VisitContinueStatement(context);
        }

        public override QueryResult VisitConversion_operator_declarator([NotNull] CSharpParser.Conversion_operator_declaratorContext context)
        {
            return base.VisitConversion_operator_declarator(context);
        }

        public override QueryResult VisitDeclarationStatement([NotNull] CSharpParser.DeclarationStatementContext context)
        {
            return base.VisitDeclarationStatement(context);
        }

        public override QueryResult VisitDefaultValueExpression([NotNull] CSharpParser.DefaultValueExpressionContext context)
        {
            return base.VisitDefaultValueExpression(context);
        }

        public override QueryResult VisitDelegate_definition([NotNull] CSharpParser.Delegate_definitionContext context)
        {
            return base.VisitDelegate_definition(context);
        }

        public override QueryResult VisitDestructor_definition([NotNull] CSharpParser.Destructor_definitionContext context)
        {
            return base.VisitDestructor_definition(context);
        }

        public override QueryResult VisitDoStatement([NotNull] CSharpParser.DoStatementContext context)
        {
            return base.VisitDoStatement(context);
        }

        public override QueryResult VisitElement_initializer([NotNull] CSharpParser.Element_initializerContext context)
        {
            return base.VisitElement_initializer(context);
        }

        public override QueryResult VisitEmbedded_statement([NotNull] CSharpParser.Embedded_statementContext context)
        {
            return base.VisitEmbedded_statement(context);
        }

        public override QueryResult VisitEnum_base([NotNull] CSharpParser.Enum_baseContext context)
        {
            return base.VisitEnum_base(context);
        }

        public override QueryResult VisitEnum_body([NotNull] CSharpParser.Enum_bodyContext context)
        {
            return base.VisitEnum_body(context);
        }

        public override QueryResult VisitEnum_definition([NotNull] CSharpParser.Enum_definitionContext context)
        {
            return base.VisitEnum_definition(context);
        }

        public override QueryResult VisitEnum_member_declaration([NotNull] CSharpParser.Enum_member_declarationContext context)
        {
            return base.VisitEnum_member_declaration(context);
        }

        public override QueryResult VisitEquality_expression([NotNull] CSharpParser.Equality_expressionContext context)
        {
            return base.VisitEquality_expression(context);
        }

        public override QueryResult VisitEvent_accessor_declarations([NotNull] CSharpParser.Event_accessor_declarationsContext context)
        {
            return base.VisitEvent_accessor_declarations(context);
        }

        public override QueryResult VisitEvent_declaration([NotNull] CSharpParser.Event_declarationContext context)
        {
            return base.VisitEvent_declaration(context);
        }

        public override QueryResult VisitException_filter([NotNull] CSharpParser.Exception_filterContext context)
        {
            return base.VisitException_filter(context);
        }

        public override QueryResult VisitExclusive_or_expression([NotNull] CSharpParser.Exclusive_or_expressionContext context)
        {
            return base.VisitExclusive_or_expression(context);
        }

        public override QueryResult VisitExplicit_anonymous_function_parameter([NotNull] CSharpParser.Explicit_anonymous_function_parameterContext context)
        {
            return base.VisitExplicit_anonymous_function_parameter(context);
        }

        public override QueryResult VisitExplicit_anonymous_function_parameter_list([NotNull] CSharpParser.Explicit_anonymous_function_parameter_listContext context)
        {
            return base.VisitExplicit_anonymous_function_parameter_list(context);
        }

        public override QueryResult VisitExpression([NotNull] CSharpParser.ExpressionContext context)
        {
            return base.VisitExpression(context);
        }

        public override QueryResult VisitExpressionStatement([NotNull] CSharpParser.ExpressionStatementContext context)
        {
            return base.VisitExpressionStatement(context);
        }

        public override QueryResult VisitExpression_list([NotNull] CSharpParser.Expression_listContext context)
        {
            return base.VisitExpression_list(context);
        }

        public override QueryResult VisitExtern_alias_directive([NotNull] CSharpParser.Extern_alias_directiveContext context)
        {
            return base.VisitExtern_alias_directive(context);
        }

        public override QueryResult VisitExtern_alias_directives([NotNull] CSharpParser.Extern_alias_directivesContext context)
        {
            return base.VisitExtern_alias_directives(context);
        }

        public override QueryResult VisitField_declaration([NotNull] CSharpParser.Field_declarationContext context)
        {
            return base.VisitField_declaration(context);
        }

        public override QueryResult VisitFinally_clause([NotNull] CSharpParser.Finally_clauseContext context)
        {
            return base.VisitFinally_clause(context);
        }

        public override QueryResult VisitFixedStatement([NotNull] CSharpParser.FixedStatementContext context)
        {
            return base.VisitFixedStatement(context);
        }

        public override QueryResult VisitFixed_parameter([NotNull] CSharpParser.Fixed_parameterContext context)
        {
            return base.VisitFixed_parameter(context);
        }

        public override QueryResult VisitFixed_parameters([NotNull] CSharpParser.Fixed_parametersContext context)
        {
            return base.VisitFixed_parameters(context);
        }

        public override QueryResult VisitFixed_pointer_declarator([NotNull] CSharpParser.Fixed_pointer_declaratorContext context)
        {
            return base.VisitFixed_pointer_declarator(context);
        }

        public override QueryResult VisitFixed_pointer_declarators([NotNull] CSharpParser.Fixed_pointer_declaratorsContext context)
        {
            return base.VisitFixed_pointer_declarators(context);
        }

        public override QueryResult VisitFixed_pointer_initializer([NotNull] CSharpParser.Fixed_pointer_initializerContext context)
        {
            return base.VisitFixed_pointer_initializer(context);
        }

        public override QueryResult VisitFixed_size_buffer_declarator([NotNull] CSharpParser.Fixed_size_buffer_declaratorContext context)
        {
            return base.VisitFixed_size_buffer_declarator(context);
        }

        public override QueryResult VisitFloating_point_type([NotNull] CSharpParser.Floating_point_typeContext context)
        {
            return base.VisitFloating_point_type(context);
        }

        public override QueryResult VisitForeachStatement([NotNull] CSharpParser.ForeachStatementContext context)
        {
            return base.VisitForeachStatement(context);
        }

        public override QueryResult VisitFormal_parameter_list([NotNull] CSharpParser.Formal_parameter_listContext context)
        {
            return base.VisitFormal_parameter_list(context);
        }

        public override QueryResult VisitForStatement([NotNull] CSharpParser.ForStatementContext context)
        {
            return base.VisitForStatement(context);
        }

        public override QueryResult VisitFor_initializer([NotNull] CSharpParser.For_initializerContext context)
        {
            return base.VisitFor_initializer(context);
        }

        public override QueryResult VisitFor_iterator([NotNull] CSharpParser.For_iteratorContext context)
        {
            return base.VisitFor_iterator(context);
        }

        public override QueryResult VisitFrom_clause([NotNull] CSharpParser.From_clauseContext context)
        {
            return base.VisitFrom_clause(context);
        }

        public override QueryResult VisitGeneral_catch_clause([NotNull] CSharpParser.General_catch_clauseContext context)
        {
            return base.VisitGeneral_catch_clause(context);
        }

        public override QueryResult VisitGeneric_dimension_specifier([NotNull] CSharpParser.Generic_dimension_specifierContext context)
        {
            return base.VisitGeneric_dimension_specifier(context);
        }

        public override QueryResult VisitGet_accessor_declaration([NotNull] CSharpParser.Get_accessor_declarationContext context)
        {
            return base.VisitGet_accessor_declaration(context);
        }

        public override QueryResult VisitGlobal_attribute_section([NotNull] CSharpParser.Global_attribute_sectionContext context)
        {
            return base.VisitGlobal_attribute_section(context);
        }

        public override QueryResult VisitGlobal_attribute_target([NotNull] CSharpParser.Global_attribute_targetContext context)
        {
            return base.VisitGlobal_attribute_target(context);
        }

        public override QueryResult VisitGotoStatement([NotNull] CSharpParser.GotoStatementContext context)
        {
            return base.VisitGotoStatement(context);
        }

        public override QueryResult VisitIdentifier([NotNull] CSharpParser.IdentifierContext context)
        {
            return base.VisitIdentifier(context);
        }

        public override QueryResult VisitIfStatement([NotNull] CSharpParser.IfStatementContext context)
        {
            return base.VisitIfStatement(context);
        }

        public override QueryResult VisitIf_body([NotNull] CSharpParser.If_bodyContext context)
        {
            return base.VisitIf_body(context);
        }

        public override QueryResult VisitImplicit_anonymous_function_parameter_list([NotNull] CSharpParser.Implicit_anonymous_function_parameter_listContext context)
        {
            return base.VisitImplicit_anonymous_function_parameter_list(context);
        }

        public override QueryResult VisitInclusive_or_expression([NotNull] CSharpParser.Inclusive_or_expressionContext context)
        {
            return base.VisitInclusive_or_expression(context);
        }

        public override QueryResult VisitIndexer_argument([NotNull] CSharpParser.Indexer_argumentContext context)
        {
            return base.VisitIndexer_argument(context);
        }

        public override QueryResult VisitIndexer_declaration([NotNull] CSharpParser.Indexer_declarationContext context)
        {
            return base.VisitIndexer_declaration(context);
        }

        public override QueryResult VisitInitializer_value([NotNull] CSharpParser.Initializer_valueContext context)
        {
            return base.VisitInitializer_value(context);
        }

        public override QueryResult VisitIntegral_type([NotNull] CSharpParser.Integral_typeContext context)
        {
            return base.VisitIntegral_type(context);
        }

        public override QueryResult VisitInterface_accessors([NotNull] CSharpParser.Interface_accessorsContext context)
        {
            return base.VisitInterface_accessors(context);
        }

        public override QueryResult VisitInterface_base([NotNull] CSharpParser.Interface_baseContext context)
        {
            return base.VisitInterface_base(context);
        }

        public override QueryResult VisitInterface_body([NotNull] CSharpParser.Interface_bodyContext context)
        {
            return base.VisitInterface_body(context);
        }

        public override QueryResult VisitInterface_definition([NotNull] CSharpParser.Interface_definitionContext context)
        {
            return base.VisitInterface_definition(context);
        }

        public override QueryResult VisitInterface_member_declaration([NotNull] CSharpParser.Interface_member_declarationContext context)
        {
            return base.VisitInterface_member_declaration(context);
        }

        public override QueryResult VisitInterface_type_list([NotNull] CSharpParser.Interface_type_listContext context)
        {
            return base.VisitInterface_type_list(context);
        }

        public override QueryResult VisitInterpolated_regular_string([NotNull] CSharpParser.Interpolated_regular_stringContext context)
        {
            return base.VisitInterpolated_regular_string(context);
        }

        public override QueryResult VisitInterpolated_regular_string_part([NotNull] CSharpParser.Interpolated_regular_string_partContext context)
        {
            return base.VisitInterpolated_regular_string_part(context);
        }

        public override QueryResult VisitInterpolated_string_expression([NotNull] CSharpParser.Interpolated_string_expressionContext context)
        {
            return base.VisitInterpolated_string_expression(context);
        }

        public override QueryResult VisitInterpolated_verbatium_string([NotNull] CSharpParser.Interpolated_verbatium_stringContext context)
        {
            return base.VisitInterpolated_verbatium_string(context);
        }

        public override QueryResult VisitInterpolated_verbatium_string_part([NotNull] CSharpParser.Interpolated_verbatium_string_partContext context)
        {
            return base.VisitInterpolated_verbatium_string_part(context);
        }

        public override QueryResult VisitIsType([NotNull] CSharpParser.IsTypeContext context)
        {
            return base.VisitIsType(context);
        }

        public override QueryResult VisitIsTypePatternArm([NotNull] CSharpParser.IsTypePatternArmContext context)
        {
            return base.VisitIsTypePatternArm(context);
        }

        public override QueryResult VisitIsTypePatternArms([NotNull] CSharpParser.IsTypePatternArmsContext context)
        {
            return base.VisitIsTypePatternArms(context);
        }

        public override QueryResult VisitKeyword([NotNull] CSharpParser.KeywordContext context)
        {
            return base.VisitKeyword(context);
        }

        public override QueryResult VisitLabeled_Statement([NotNull] CSharpParser.Labeled_StatementContext context)
        {
            return base.VisitLabeled_Statement(context);
        }

        public override QueryResult VisitLambda_expression([NotNull] CSharpParser.Lambda_expressionContext context)
        {
            return base.VisitLambda_expression(context);
        }

        public override QueryResult VisitLet_clause([NotNull] CSharpParser.Let_clauseContext context)
        {
            return base.VisitLet_clause(context);
        }

        public override QueryResult VisitLiteral([NotNull] CSharpParser.LiteralContext context)
        {
            return base.VisitLiteral(context);
        }

        public override QueryResult VisitLiteralAccessExpression([NotNull] CSharpParser.LiteralAccessExpressionContext context)
        {
            return base.VisitLiteralAccessExpression(context);
        }

        public override QueryResult VisitLiteralExpression([NotNull] CSharpParser.LiteralExpressionContext context)
        {
            return base.VisitLiteralExpression(context);
        }

        public override QueryResult VisitLocal_constant_declaration([NotNull] CSharpParser.Local_constant_declarationContext context)
        {
            return base.VisitLocal_constant_declaration(context);
        }

        public override QueryResult VisitLocal_function_body([NotNull] CSharpParser.Local_function_bodyContext context)
        {
            return base.VisitLocal_function_body(context);
        }

        public override QueryResult VisitLocal_function_declaration([NotNull] CSharpParser.Local_function_declarationContext context)
        {
            return base.VisitLocal_function_declaration(context);
        }

        public override QueryResult VisitLocal_function_header([NotNull] CSharpParser.Local_function_headerContext context)
        {
            return base.VisitLocal_function_header(context);
        }

        public override QueryResult VisitLocal_function_modifiers([NotNull] CSharpParser.Local_function_modifiersContext context)
        {
            return base.VisitLocal_function_modifiers(context);
        }

        public override QueryResult VisitLocal_variable_declaration([NotNull] CSharpParser.Local_variable_declarationContext context)
        {
            return base.VisitLocal_variable_declaration(context);
        }

        public override QueryResult VisitLocal_variable_declarator([NotNull] CSharpParser.Local_variable_declaratorContext context)
        {
            return base.VisitLocal_variable_declarator(context);
        }

        public override QueryResult VisitLocal_variable_initializer([NotNull] CSharpParser.Local_variable_initializerContext context)
        {
            return base.VisitLocal_variable_initializer(context);
        }

        public override QueryResult VisitLocal_variable_type([NotNull] CSharpParser.Local_variable_typeContext context)
        {
            return base.VisitLocal_variable_type(context);
        }

        public override QueryResult VisitLockStatement([NotNull] CSharpParser.LockStatementContext context)
        {
            return base.VisitLockStatement(context);
        }

        public override QueryResult VisitMemberAccessExpression([NotNull] CSharpParser.MemberAccessExpressionContext context)
        {
            return base.VisitMemberAccessExpression(context);
        }

        public override QueryResult VisitMember_access([NotNull] CSharpParser.Member_accessContext context)
        {
            return base.VisitMember_access(context);
        }

        public override QueryResult VisitMember_declarator([NotNull] CSharpParser.Member_declaratorContext context)
        {
            return base.VisitMember_declarator(context);
        }

        public override QueryResult VisitMember_declarator_list([NotNull] CSharpParser.Member_declarator_listContext context)
        {
            return base.VisitMember_declarator_list(context);
        }

        public override QueryResult VisitMember_initializer([NotNull] CSharpParser.Member_initializerContext context)
        {
            return base.VisitMember_initializer(context);
        }

        public override QueryResult VisitMember_initializer_list([NotNull] CSharpParser.Member_initializer_listContext context)
        {
            return base.VisitMember_initializer_list(context);
        }

        public override QueryResult VisitMember_name([NotNull] CSharpParser.Member_nameContext context)
        {
            return base.VisitMember_name(context);
        }

        public override QueryResult VisitMethod_body([NotNull] CSharpParser.Method_bodyContext context)
        {
            return base.VisitMethod_body(context);
        }

        public override QueryResult VisitMethod_declaration([NotNull] CSharpParser.Method_declarationContext context)
        {
            return base.VisitMethod_declaration(context);
        }

        public override QueryResult VisitMethod_invocation([NotNull] CSharpParser.Method_invocationContext context)
        {
            return base.VisitMethod_invocation(context);
        }

        public override QueryResult VisitMethod_member_name([NotNull] CSharpParser.Method_member_nameContext context)
        {
            return base.VisitMethod_member_name(context);
        }

        public override QueryResult VisitMultiplicative_expression([NotNull] CSharpParser.Multiplicative_expressionContext context)
        {
            return base.VisitMultiplicative_expression(context);
        }

        public override QueryResult VisitNameofExpression([NotNull] CSharpParser.NameofExpressionContext context)
        {
            return base.VisitNameofExpression(context);
        }

        public override QueryResult VisitNamespace_body([NotNull] CSharpParser.Namespace_bodyContext context)
        {
            return base.VisitNamespace_body(context);
        }

        public override QueryResult VisitNamespace_declaration([NotNull] CSharpParser.Namespace_declarationContext context)
        {
            return base.VisitNamespace_declaration(context);
        }

        public override QueryResult VisitNamespace_member_declaration([NotNull] CSharpParser.Namespace_member_declarationContext context)
        {
            return base.VisitNamespace_member_declaration(context);
        }

        public override QueryResult VisitNamespace_member_declarations([NotNull] CSharpParser.Namespace_member_declarationsContext context)
        {
            return base.VisitNamespace_member_declarations(context);
        }

        public override QueryResult VisitNamespace_or_type_name([NotNull] CSharpParser.Namespace_or_type_nameContext context)
        {
            return base.VisitNamespace_or_type_name(context);
        }

        public override QueryResult VisitNon_assignment_expression([NotNull] CSharpParser.Non_assignment_expressionContext context)
        {
            return base.VisitNon_assignment_expression(context);
        }

        public override QueryResult VisitNull_coalescing_expression([NotNull] CSharpParser.Null_coalescing_expressionContext context)
        {
            return base.VisitNull_coalescing_expression(context);
        }

        public override QueryResult VisitNumeric_type([NotNull] CSharpParser.Numeric_typeContext context)
        {
            return base.VisitNumeric_type(context);
        }

        public override QueryResult VisitObjectCreationExpression([NotNull] CSharpParser.ObjectCreationExpressionContext context)
        {
            return base.VisitObjectCreationExpression(context);
        }

        public override QueryResult VisitObject_creation_expression([NotNull] CSharpParser.Object_creation_expressionContext context)
        {
            return base.VisitObject_creation_expression(context);
        }

        public override QueryResult VisitObject_initializer([NotNull] CSharpParser.Object_initializerContext context)
        {
            return base.VisitObject_initializer(context);
        }

        public override QueryResult VisitObject_or_collection_initializer([NotNull] CSharpParser.Object_or_collection_initializerContext context)
        {
            return base.VisitObject_or_collection_initializer(context);
        }

        public override QueryResult VisitOperator_declaration([NotNull] CSharpParser.Operator_declarationContext context)
        {
            return base.VisitOperator_declaration(context);
        }

        public override QueryResult VisitOrderby_clause([NotNull] CSharpParser.Orderby_clauseContext context)
        {
            return base.VisitOrderby_clause(context);
        }

        public override QueryResult VisitOrdering([NotNull] CSharpParser.OrderingContext context)
        {
            return base.VisitOrdering(context);
        }

        public override QueryResult VisitOverloadable_operator([NotNull] CSharpParser.Overloadable_operatorContext context)
        {
            return base.VisitOverloadable_operator(context);
        }

        public override QueryResult VisitParameter_array([NotNull] CSharpParser.Parameter_arrayContext context)
        {
            return base.VisitParameter_array(context);
        }

        public override QueryResult VisitParameter_modifier([NotNull] CSharpParser.Parameter_modifierContext context)
        {
            return base.VisitParameter_modifier(context);
        }

        public override QueryResult VisitParenthesisExpressions([NotNull] CSharpParser.ParenthesisExpressionsContext context)
        {
            return base.VisitParenthesisExpressions(context);
        }

        public override QueryResult VisitPointer_type([NotNull] CSharpParser.Pointer_typeContext context)
        {
            return base.VisitPointer_type(context);
        }

        public override QueryResult VisitPredefined_type([NotNull] CSharpParser.Predefined_typeContext context)
        {
            return base.VisitPredefined_type(context);
        }

        public override QueryResult VisitPrimary_constraint([NotNull] CSharpParser.Primary_constraintContext context)
        {
            return base.VisitPrimary_constraint(context);
        }

        public override QueryResult VisitPrimary_expression([NotNull] CSharpParser.Primary_expressionContext context)
        {
            return base.VisitPrimary_expression(context);
        }

        public override QueryResult VisitPrimary_expression_start([NotNull] CSharpParser.Primary_expression_startContext context)
        {
            return base.VisitPrimary_expression_start(context);
        }

        public override QueryResult VisitProperty_declaration([NotNull] CSharpParser.Property_declarationContext context)
        {
            return base.VisitProperty_declaration(context);
        }

        public override QueryResult VisitQualified_alias_member([NotNull] CSharpParser.Qualified_alias_memberContext context)
        {
            return base.VisitQualified_alias_member(context);
        }

        public override QueryResult VisitQualified_identifier([NotNull] CSharpParser.Qualified_identifierContext context)
        {
            return base.VisitQualified_identifier(context);
        }

        public override QueryResult VisitQuery_body([NotNull] CSharpParser.Query_bodyContext context)
        {
            return base.VisitQuery_body(context);
        }

        public override QueryResult VisitQuery_body_clause([NotNull] CSharpParser.Query_body_clauseContext context)
        {
            return base.VisitQuery_body_clause(context);
        }

        public override QueryResult VisitQuery_continuation([NotNull] CSharpParser.Query_continuationContext context)
        {
            return base.VisitQuery_continuation(context);
        }

        public override QueryResult VisitQuery_expression([NotNull] CSharpParser.Query_expressionContext context)
        {
            return base.VisitQuery_expression(context);
        }

        public override QueryResult VisitRange_expression([NotNull] CSharpParser.Range_expressionContext context)
        {
            return base.VisitRange_expression(context);
        }

        public override QueryResult VisitRank_specifier([NotNull] CSharpParser.Rank_specifierContext context)
        {
            return base.VisitRank_specifier(context);
        }

        public override QueryResult VisitRelational_expression([NotNull] CSharpParser.Relational_expressionContext context)
        {
            return base.VisitRelational_expression(context);
        }

        public override QueryResult VisitRemove_accessor_declaration([NotNull] CSharpParser.Remove_accessor_declarationContext context)
        {
            return base.VisitRemove_accessor_declaration(context);
        }

        public override QueryResult VisitResource_acquisition([NotNull] CSharpParser.Resource_acquisitionContext context)
        {
            return base.VisitResource_acquisition(context);
        }

        public override QueryResult VisitReturnStatement([NotNull] CSharpParser.ReturnStatementContext context)
        {
            return base.VisitReturnStatement(context);
        }

        public override QueryResult VisitReturn_type([NotNull] CSharpParser.Return_typeContext context)
        {
            return base.VisitReturn_type(context);
        }

        public override QueryResult VisitRight_arrow([NotNull] CSharpParser.Right_arrowContext context)
        {
            return base.VisitRight_arrow(context);
        }

        public override QueryResult VisitRight_shift([NotNull] CSharpParser.Right_shiftContext context)
        {
            return base.VisitRight_shift(context);
        }

        public override QueryResult VisitRight_shift_assignment([NotNull] CSharpParser.Right_shift_assignmentContext context)
        {
            return base.VisitRight_shift_assignment(context);
        }

        public override QueryResult VisitSecondary_constraints([NotNull] CSharpParser.Secondary_constraintsContext context)
        {
            return base.VisitSecondary_constraints(context);
        }

        public override QueryResult VisitSelect_or_group_clause([NotNull] CSharpParser.Select_or_group_clauseContext context)
        {
            return base.VisitSelect_or_group_clause(context);
        }

        public override QueryResult VisitSet_accessor_declaration([NotNull] CSharpParser.Set_accessor_declarationContext context)
        {
            return base.VisitSet_accessor_declaration(context);
        }

        public override QueryResult VisitShift_expression([NotNull] CSharpParser.Shift_expressionContext context)
        {
            return base.VisitShift_expression(context);
        }

        public override QueryResult VisitSimpleNameExpression([NotNull] CSharpParser.SimpleNameExpressionContext context)
        {
            return base.VisitSimpleNameExpression(context);
        }

        public override QueryResult VisitSimple_embedded_statement([NotNull] CSharpParser.Simple_embedded_statementContext context)
        {
            return base.VisitSimple_embedded_statement(context);
        }

        public override QueryResult VisitSimple_type([NotNull] CSharpParser.Simple_typeContext context)
        {
            return base.VisitSimple_type(context);
        }

        public override QueryResult VisitSizeofExpression([NotNull] CSharpParser.SizeofExpressionContext context)
        {
            return base.VisitSizeofExpression(context);
        }

        public override QueryResult VisitSpecific_catch_clause([NotNull] CSharpParser.Specific_catch_clauseContext context)
        {
            return base.VisitSpecific_catch_clause(context);
        }

        public override QueryResult VisitStackalloc_initializer([NotNull] CSharpParser.Stackalloc_initializerContext context)
        {
            return base.VisitStackalloc_initializer(context);
        }

        public override QueryResult VisitStatement([NotNull] CSharpParser.StatementContext context)
        {
            return base.VisitStatement(context);
        }

        public override QueryResult VisitStatement_list([NotNull] CSharpParser.Statement_listContext context)
        {
            return base.VisitStatement_list(context);
        }

        public override QueryResult VisitString_literal([NotNull] CSharpParser.String_literalContext context)
        {
            return base.VisitString_literal(context);
        }

        public override QueryResult VisitStruct_body([NotNull] CSharpParser.Struct_bodyContext context)
        {
            return base.VisitStruct_body(context);
        }

        public override QueryResult VisitStruct_definition([NotNull] CSharpParser.Struct_definitionContext context)
        {
            return base.VisitStruct_definition(context);
        }

        public override QueryResult VisitStruct_interfaces([NotNull] CSharpParser.Struct_interfacesContext context)
        {
            return base.VisitStruct_interfaces(context);
        }

        public override QueryResult VisitStruct_member_declaration([NotNull] CSharpParser.Struct_member_declarationContext context)
        {
            return base.VisitStruct_member_declaration(context);
        }

        public override QueryResult VisitSwitchStatement([NotNull] CSharpParser.SwitchStatementContext context)
        {
            return base.VisitSwitchStatement(context);
        }

        public override QueryResult VisitSwitch_expression([NotNull] CSharpParser.Switch_expressionContext context)
        {
            return base.VisitSwitch_expression(context);
        }

        public override QueryResult VisitSwitch_expression_arm([NotNull] CSharpParser.Switch_expression_armContext context)
        {
            return base.VisitSwitch_expression_arm(context);
        }

        public override QueryResult VisitSwitch_expression_arms([NotNull] CSharpParser.Switch_expression_armsContext context)
        {
            return base.VisitSwitch_expression_arms(context);
        }

        public override QueryResult VisitSwitch_label([NotNull] CSharpParser.Switch_labelContext context)
        {
            return base.VisitSwitch_label(context);
        }

        public override QueryResult VisitSwitch_section([NotNull] CSharpParser.Switch_sectionContext context)
        {
            return base.VisitSwitch_section(context);
        }

        public override QueryResult VisitTheEmptyStatement([NotNull] CSharpParser.TheEmptyStatementContext context)
        {
            return base.VisitTheEmptyStatement(context);
        }

        public override QueryResult VisitThisReferenceExpression([NotNull] CSharpParser.ThisReferenceExpressionContext context)
        {
            return base.VisitThisReferenceExpression(context);
        }

        public override QueryResult VisitThrowable_expression([NotNull] CSharpParser.Throwable_expressionContext context)
        {
            return base.VisitThrowable_expression(context);
        }

        public override QueryResult VisitThrowStatement([NotNull] CSharpParser.ThrowStatementContext context)
        {
            return base.VisitThrowStatement(context);
        }

        public override QueryResult VisitThrow_expression([NotNull] CSharpParser.Throw_expressionContext context)
        {
            return base.VisitThrow_expression(context);
        }

        public override QueryResult VisitTryStatement([NotNull] CSharpParser.TryStatementContext context)
        {
            return base.VisitTryStatement(context);
        }

        public override QueryResult VisitTupleExpression([NotNull] CSharpParser.TupleExpressionContext context)
        {
            return base.VisitTupleExpression(context);
        }

        public override QueryResult VisitTuple_element([NotNull] CSharpParser.Tuple_elementContext context)
        {
            return base.VisitTuple_element(context);
        }

        public override QueryResult VisitTuple_type([NotNull] CSharpParser.Tuple_typeContext context)
        {
            return base.VisitTuple_type(context);
        }

        public override QueryResult VisitTyped_member_declaration([NotNull] CSharpParser.Typed_member_declarationContext context)
        {
            return base.VisitTyped_member_declaration(context);
        }

        public override QueryResult VisitTypeofExpression([NotNull] CSharpParser.TypeofExpressionContext context)
        {
            return base.VisitTypeofExpression(context);
        }

        public override QueryResult VisitType_([NotNull] CSharpParser.Type_Context context)
        {
            return base.VisitType_(context);
        }

        public override QueryResult VisitType_argument_list([NotNull] CSharpParser.Type_argument_listContext context)
        {
            return base.VisitType_argument_list(context);
        }

        public override QueryResult VisitType_declaration([NotNull] CSharpParser.Type_declarationContext context)
        {
            return base.VisitType_declaration(context);
        }

        public override QueryResult VisitType_parameter([NotNull] CSharpParser.Type_parameterContext context)
        {
            return base.VisitType_parameter(context);
        }

        public override QueryResult VisitType_parameter_constraints([NotNull] CSharpParser.Type_parameter_constraintsContext context)
        {
            return base.VisitType_parameter_constraints(context);
        }

        public override QueryResult VisitType_parameter_constraints_clause([NotNull] CSharpParser.Type_parameter_constraints_clauseContext context)
        {
            return base.VisitType_parameter_constraints_clause(context);
        }

        public override QueryResult VisitType_parameter_constraints_clauses([NotNull] CSharpParser.Type_parameter_constraints_clausesContext context)
        {
            return base.VisitType_parameter_constraints_clauses(context);
        }

        public override QueryResult VisitType_parameter_list([NotNull] CSharpParser.Type_parameter_listContext context)
        {
            return base.VisitType_parameter_list(context);
        }

        public override QueryResult VisitUnary_expression([NotNull] CSharpParser.Unary_expressionContext context)
        {
            return base.VisitUnary_expression(context);
        }

        public override QueryResult VisitUnbound_type_name([NotNull] CSharpParser.Unbound_type_nameContext context)
        {
            return base.VisitUnbound_type_name(context);
        }

        public override QueryResult VisitUncheckedExpression([NotNull] CSharpParser.UncheckedExpressionContext context)
        {
            return base.VisitUncheckedExpression(context);
        }

        public override QueryResult VisitUncheckedStatement([NotNull] CSharpParser.UncheckedStatementContext context)
        {
            return base.VisitUncheckedStatement(context);
        }

        public override QueryResult VisitUnsafeStatement([NotNull] CSharpParser.UnsafeStatementContext context)
        {
            return base.VisitUnsafeStatement(context);
        }

        public override QueryResult VisitUsingAliasDirective([NotNull] CSharpParser.UsingAliasDirectiveContext context)
        {
            return base.VisitUsingAliasDirective(context);
        }

        public override QueryResult VisitUsingNamespaceDirective([NotNull] CSharpParser.UsingNamespaceDirectiveContext context)
        {
            return base.VisitUsingNamespaceDirective(context);
        }

        public override QueryResult VisitUsingStatement([NotNull] CSharpParser.UsingStatementContext context)
        {
            return base.VisitUsingStatement(context);
        }

        public override QueryResult VisitUsingStaticDirective([NotNull] CSharpParser.UsingStaticDirectiveContext context)
        {
            return base.VisitUsingStaticDirective(context);
        }

        public override QueryResult VisitUsing_directive([NotNull] CSharpParser.Using_directiveContext context)
        {
            return base.VisitUsing_directive(context);
        }

        public override QueryResult VisitUsing_directives([NotNull] CSharpParser.Using_directivesContext context)
        {
            return base.VisitUsing_directives(context);
        }

        public override QueryResult VisitVariable_declarator([NotNull] CSharpParser.Variable_declaratorContext context)
        {
            return base.VisitVariable_declarator(context);
        }

        public override QueryResult VisitVariable_declarators([NotNull] CSharpParser.Variable_declaratorsContext context)
        {
            return base.VisitVariable_declarators(context);
        }

        public override QueryResult VisitVariable_initializer([NotNull] CSharpParser.Variable_initializerContext context)
        {
            return base.VisitVariable_initializer(context);
        }

        public override QueryResult VisitVariance_annotation([NotNull] CSharpParser.Variance_annotationContext context)
        {
            return base.VisitVariance_annotation(context);
        }

        public override QueryResult VisitVariant_type_parameter([NotNull] CSharpParser.Variant_type_parameterContext context)
        {
            return base.VisitVariant_type_parameter(context);
        }

        public override QueryResult VisitVariant_type_parameter_list([NotNull] CSharpParser.Variant_type_parameter_listContext context)
        {
            return base.VisitVariant_type_parameter_list(context);
        }

        public override QueryResult VisitWhere_clause([NotNull] CSharpParser.Where_clauseContext context)
        {
            return base.VisitWhere_clause(context);
        }

        public override QueryResult VisitWhileStatement([NotNull] CSharpParser.WhileStatementContext context)
        {
            return base.VisitWhileStatement(context);
        }

        public override QueryResult VisitYieldStatement([NotNull] CSharpParser.YieldStatementContext context)
        {
            return base.VisitYieldStatement(context);
        }
    }
}
