grammar CSharpParser;
@parser::header {#pragma warning disable 3021}

// $antlr-format alignTrailingComments true, columnLimit 150, minEmptyLines 1, maxEmptyLinesToKeep 1, reflowComments false, useTab false
// $antlr-format allowShortRulesOnASingleLine false, allowShortBlocksOnASingleLine true, alignSemicolons hanging, alignColons hanging

parser grammar CSharpParser;
@parser::header {#pragma warning disable 3021}

options {
    tokenVocab = CSharpLexer;
    superClass = Aspects.Parsers.CSharp.Base.CSharpParserBase;
}

// entry point
compilation_unit
    : BYTE_ORDER_MARK? extern_alias_directives? using_directives? global_attribute_section* namespace_member_declarations? EOF
    ;

//B.2 Syntactic grammar

//B.2.1 Basic concepts

namespace_or_type_name
    : (identifier type_argument_list? | qualified_alias_member) (
        '.' identifier type_argument_list?
    )*
    ;

//B.2.2 Types
type_
    : base_type ('?' | rank_specifier | '*')*
    ;

base_type
    : simple_type
    | class_type // represents types: enum, class, interface, delegate, type_parameter
    | VOID '*'
    | tuple_type
    ;

tuple_type
    : '(' tuple_element (',' tuple_element)+ ')'
    ;

tuple_element
    : type_ identifier?
    ;

simple_type
    : numeric_type
    | BOOL
    ;

numeric_type
    : integral_type
    | floating_point_type
    | DECIMAL
    ;

integral_type
    : SBYTE
    | BYTE
    | SHORT
    | USHORT
    | INT
    | UINT
    | LONG
    | ULONG
    | CHAR
    ;

floating_point_type
    : FLOAT
    | DOUBLE
    ;

/** namespace_or_type_name, OBJECT, STRING */
class_type
    : namespace_or_type_name
    | OBJECT
    | DYNAMIC
    | STRING
    ;

type_argument_list
    : '<' type_ (',' type_)* '>'
    ;

//B.2.4 Expressions
argument_list
    : argument (',' argument)*
    ;

argument
    : (identifier ':')? refout = (REF | OUT | IN)? (expression | (VAR | type_) expression)
    ;

expression
    : assignment
    | non_assignment_expression
    | REF non_assignment_expression
    ;

non_assignment_expression
    : lambda_expression
    | query_expression
    | conditional_expression
    ;

assignment
    : unary_expression assignment_operator expression
    | unary_expression '??=' throwable_expression
    ;

assignment_operator
    : '='
    | '+='
    | '-='
    | '*='
    | '/='
    | '%='
    | '&='
    | '|='
    | '^='
    | '<<='
    | right_shift_assignment
    ;

conditional_expression
    : null_coalescing_expression ('?' throwable_expression ':' throwable_expression)?
    ;

null_coalescing_expression
    : conditional_or_expression ('??' (null_coalescing_expression | throw_expression))?
    ;

conditional_or_expression
    : conditional_and_expression (OP_OR conditional_and_expression)*
    ;

conditional_and_expression
    : inclusive_or_expression (OP_AND inclusive_or_expression)*
    ;

Interpolated_Regular_String_Start
    : '$"'
    ;

// the following three lexical rules are context sensitive, see details below

Interpolated_Regular_String_Mid
    : Interpolated_Regular_String_Element+
    ;

Regular_Interpolation_Format
    : ':' Interpolated_Regular_String_Element+
    ;

Interpolated_Regular_String_End
    : '"'
    ;

fragment Interpolated_Regular_String_Element
    : Interpolated_Regular_String_Character
    | Simple_Escape_Sequence
    | Hexadecimal_Escape_Sequence
    | Unicode_Escape_Sequence
    | Open_Brace_Escape_Sequence
    | Close_Brace_Escape_Sequence
    ;

fragment Interpolated_Regular_String_Character
    // Any character except " (U+0022), \\ (U+005C),
    // { (U+007B), } (U+007D), and New_Line_Character.
    : ~["\\{}\u000D\u000A\u0085\u2028\u2029]
    ;

// interpolated verbatim string expressions

interpolated_verbatim_string_expression
    : Interpolated_Verbatim_String_Start Interpolated_Verbatim_String_Mid?
      ('{' verbatim_interpolation '}' Interpolated_Verbatim_String_Mid?)*
      Interpolated_Verbatim_String_End
    ;

parenthesized_pattern
    : '(' pattern ')'
    ;

Interpolated_Verbatim_String_Start
    : '$@"'
    | '@$"'
    ;

// the following three lexical rules are context sensitive, see details below

Interpolated_Verbatim_String_Mid
    : Interpolated_Verbatim_String_Element+
    ;

Verbatim_Interpolation_Format
    : ':' Interpolated_Verbatim_String_Element+
    ;

Interpolated_Verbatim_String_End
    : '"'
    ;

fragment Interpolated_Verbatim_String_Element
    : Interpolated_Verbatim_String_Character
    | Quote_Escape_Sequence
    | Open_Brace_Escape_Sequence
    | Close_Brace_Escape_Sequence
    ;

fragment Interpolated_Verbatim_String_Character
    : ~["{}]    // Any character except " (U+0022), { (U+007B) and } (U+007D)
    ;

// lexical fragments used by both regular and verbatim interpolated strings

fragment Open_Brace_Escape_Sequence
    : '{{'
    ;

fragment Close_Brace_Escape_Sequence
    : '}}'
    ;

// Source: §12.8.4 Simple names
simple_name
    : identifier type_argument_list?
    ;

switch_expression
    : range_expression ('switch' '{' (switch_expression_arms ','?)? '}')?
    ;

switch_expression_arms
    : switch_expression_arm (',' switch_expression_arm)*
    ;

switch_expression_arm
    : expression case_guard? right_arrow throwable_expression
    ;

range_expression
    : unary_expression
    | unary_expression? OP_RANGE unary_expression?
    ;

// https://msdn.microsoft.com/library/6a71f45d(v=vs.110).aspx
unary_expression
    : cast_expression
    | primary_expression
    | '+' unary_expression
    | '-' unary_expression
    | BANG unary_expression
    | '~' unary_expression
    | '++' unary_expression
    | '--' unary_expression
    | AWAIT unary_expression // C# 5
    | '&' unary_expression
    | '*' unary_expression
    | '^' unary_expression // C# 8 ranges
    ;

cast_expression
    : OPEN_PARENS type_ CLOSE_PARENS unary_expression
    ;

// Source: §12.8.7.1 General
member_access
    : primary_expression '.' identifier type_argument_list?
    | predefined_type '.' identifier type_argument_list?
    | qualified_alias_member '.' identifier type_argument_list?
    ;

predefined_type
    : 'bool' | 'byte' | 'char' | 'decimal' | 'double' | 'float' | 'int'
    | 'long' | 'object' | 'sbyte' | 'short' | 'string' | 'uint' | 'ulong'
    | 'ushort'
    ;

// Source: §12.8.8 Null Conditional Member Access
null_conditional_member_access
    : primary_expression '?' '.' identifier type_argument_list?
      dependent_access*
    ;
    
dependent_access
    : '.' identifier type_argument_list?    // member access
    | '[' argument_list ']'                 // element access
    | '(' argument_list? ')'                // invocation
    ;

member_access
    : '?'? '.' identifier type_argument_list?
    ;

// Source: §12.8.9.1 General
invocation_expression
    : primary_expression '(' argument_list? ')'
    ;

// Source: §12.8.10 Null Conditional Invocation Expression
null_conditional_invocation_expression
    : null_conditional_member_access '(' argument_list? ')'
    | null_conditional_element_access '(' argument_list? ')'
    ;

// Source: §12.8.11.1 General
element_access
    : primary_no_array_creation_expression '[' argument_list ']'
    ;

expression_list
    : expression (',' expression)*
    ;

// Source: §12.8.13 This access
this_access
    : 'this'
    ;

// Source: §12.8.14 Base access
base_access
    : 'base' '.' identifier type_argument_list?
    | 'base' '[' argument_list ']'
    ;

// Source: §12.8.15 Postfix increment and decrement operators
post_increment_expression
    : primary_expression '++'
    ;

post_decrement_expression
    : primary_expression '--'
    ;

// Source: §12.8.16.2 Object creation expressions
object_creation_expression
    : 'new' type '(' argument_list? ')' object_or_collection_initializer?
    | 'new' type object_or_collection_initializer
    ;

object_or_collection_initializer
    : object_initializer
    | collection_initializer
    ;

object_initializer
    : OPEN_BRACE (member_initializer_list ','?)? CLOSE_BRACE
    ;

member_initializer_list
    : member_initializer (',' member_initializer)*
    ;

member_initializer
    : (identifier | '[' expression ']') '=' initializer_value // C# 6
    ;

initializer_value
    : expression
    | object_or_collection_initializer
    ;

collection_initializer
    : OPEN_BRACE element_initializer (',' element_initializer)* ','? CLOSE_BRACE
    ;

element_initializer
    : non_assignment_expression
    | OPEN_BRACE expression_list CLOSE_BRACE
    ;

anonymous_object_initializer
    : OPEN_BRACE (member_declarator_list ','?)? CLOSE_BRACE
    ;

member_declarator_list
    : member_declarator (',' member_declarator)*
    ;

member_declarator
    : primary_expression
    | identifier '=' expression
    ;

unbound_type_name
    : identifier (generic_dimension_specifier? | '::' identifier generic_dimension_specifier?) (
        '.' identifier generic_dimension_specifier?
    )*
    ;

generic_dimension_specifier
    : '<' ','* '>'
    ;

isType
    : base_type (rank_specifier | '*')* '?'? isTypePatternArms? identifier?
    ;

isTypePatternArms
    : '{' isTypePatternArm (',' isTypePatternArm)* '}'
    ;

isTypePatternArm
    : identifier ':' expression
    ;

unchecked_expression
    : 'unchecked' '(' expression ')'
    ;

// Source: §12.8.20 Default value expressions
default_value_expression
    : explictly_typed_default
    | default_literal
    ;

explictly_typed_default
    : 'default' '(' type ')'
    ;

default_literal
    : 'default'
    ;

// Source: §12.8.21 Stack allocation
stackalloc_expression
    : 'stackalloc' unmanaged_type '[' expression ']'
    | 'stackalloc' unmanaged_type? '[' constant_expression? ']'
      stackalloc_initializer
    ;

stackalloc_initializer
     : '{' stackalloc_initializer_element_list '}'
     ;

stackalloc_initializer_element_list
     : stackalloc_element_initializer (',' stackalloc_element_initializer)* ','?
     ;
    
stackalloc_element_initializer
    : expression
    ;

// Source: §12.8.22 The nameof operator
nameof_expression
    : 'nameof' '(' named_entity ')'
    ;
    
named_entity
    : named_entity_target ('.' identifier type_argument_list?)*
    ;
    
named_entity_target
    : simple_name
    | 'this'
    | 'base'
    | predefined_type 
    | qualified_alias_member
    ;

// Source: §12.9.1 General
unary_expression
    : primary_expression
    | '+' unary_expression
    | '-' unary_expression
    | '!' unary_expression
    | '~' unary_expression
    | pre_increment_expression
    | pre_decrement_expression
    | cast_expression
    | await_expression
    | pointer_indirection_expression    // unsafe code support
    | addressof_expression              // unsafe code support
    ;

// Source: §12.9.6 Prefix increment and decrement operators
pre_increment_expression
    : '++' unary_expression
    ;

pre_decrement_expression
    : '--' unary_expression
    ;

// Source: §12.9.7 Cast expressions
cast_expression
    : '(' type ')' unary_expression
    ;

// Source: §12.9.8.1 General
await_expression
    : 'await' unary_expression
    ;

// Source: §12.10.1 General
multiplicative_expression
    : unary_expression
    | multiplicative_expression '*' unary_expression
    | multiplicative_expression '/' unary_expression
    | multiplicative_expression '%' unary_expression
    ;

additive_expression
    : multiplicative_expression
    | additive_expression '+' multiplicative_expression
    | additive_expression '-' multiplicative_expression
    ;

// Source: §12.11 Shift operators
shift_expression
    : additive_expression
    | shift_expression '<<' additive_expression
    | shift_expression right_shift additive_expression
    ;

// Source: §12.12.1 General
relational_expression
    : shift_expression
    | relational_expression '<' shift_expression
    | relational_expression '>' shift_expression
    | relational_expression '<=' shift_expression
    | relational_expression '>=' shift_expression
    | relational_expression 'is' type
    | relational_expression 'is' pattern
    | relational_expression 'as' type
    ;

equality_expression
    : relational_expression
    | equality_expression '==' relational_expression
    | equality_expression '!=' relational_expression
    ;

// Source: §12.13.1 General
and_expression
    : equality_expression
    | and_expression '&' equality_expression
    ;

exclusive_or_expression
    : and_expression
    | exclusive_or_expression '^' and_expression
    ;

inclusive_or_expression
    : exclusive_or_expression
    | inclusive_or_expression '|' exclusive_or_expression
    ;

// Source: §12.14.1 General
conditional_and_expression
    : inclusive_or_expression
    | conditional_and_expression '&&' inclusive_or_expression
    ;

conditional_or_expression
    : conditional_and_expression
    | conditional_or_expression '||' conditional_and_expression
    ;

// Source: §12.15 The null coalescing operator
null_coalescing_expression
    : conditional_or_expression
    | conditional_or_expression '??' null_coalescing_expression
    | throw_expression
    ;

// Source: §12.16 The throw expression operator
throw_expression
    : 'throw' null_coalescing_expression
    ;

// Source: §12.17 Declaration expressions
declaration_expression
    : local_variable_type identifier
    ;

local_variable_type
    : type
    | 'var'
    ;

// Source: §12.18 Conditional operator
conditional_expression
    : null_coalescing_expression
    | null_coalescing_expression '?' expression ':' expression
    | null_coalescing_expression '?' 'ref' variable_reference ':'
      'ref' variable_reference
    ;

// Source: §12.19.1 General
lambda_expression
    : ASYNC? anonymous_function_signature right_arrow anonymous_function_body
    ;

anonymous_function_signature
    : OPEN_PARENS CLOSE_PARENS
    | OPEN_PARENS explicit_anonymous_function_parameter_list CLOSE_PARENS
    | OPEN_PARENS implicit_anonymous_function_parameter_list CLOSE_PARENS
    | identifier
    ;

explicit_anonymous_function_parameter_list
    : explicit_anonymous_function_parameter (',' explicit_anonymous_function_parameter)*
    ;

explicit_anonymous_function_parameter
    : refout = (REF | OUT | IN)? type_ identifier
    ;

implicit_anonymous_function_parameter_list
    : identifier (',' identifier)*
    ;

anonymous_function_body
    : throwable_expression
    | block
    ;

query_expression
    : from_clause query_body
    ;

from_clause
    : FROM type_? identifier IN expression
    ;

query_body
    : query_body_clause* select_or_group_clause query_continuation?
    ;

query_body_clause
    : from_clause
    | let_clause
    | where_clause
    | combined_join_clause
    | orderby_clause
    ;

let_clause
    : LET identifier '=' expression
    ;

where_clause
    : WHERE expression
    ;

combined_join_clause
    : JOIN type_? identifier IN expression ON expression EQUALS expression (INTO identifier)?
    ;

orderby_clause
    : ORDERBY ordering (',' ordering)*
    ;

ordering
    : expression dir = (ASCENDING | DESCENDING)?
    ;

select_or_group_clause
    : SELECT expression
    | GROUP expression BY expression
    ;

query_continuation
    : INTO identifier query_body
    ;

//B.2.5 Statements
statement
    : labeled_Statement
    | declarationStatement
    | embedded_statement
    ;

declarationStatement
    : local_variable_declaration ';'
    | local_constant_declaration ';'
    | local_function_declaration
    ;

local_function_declaration
    : local_function_header local_function_body
    ;

local_function_header
    : local_function_modifiers? return_type identifier type_parameter_list? OPEN_PARENS formal_parameter_list? CLOSE_PARENS
        type_parameter_constraints_clauses?
    ;

local_function_modifiers
    : (ASYNC | UNSAFE) STATIC?
    | STATIC (ASYNC | UNSAFE)
    ;

local_function_body
    : block
    | right_arrow throwable_expression ';'
    ;

labeled_Statement
    : identifier ':' statement
    ;

embedded_statement
    : block
    | simple_embedded_statement
    ;

simple_embedded_statement
    : ';'            # theEmptyStatement
    | expression ';' # expressionStatement

    // selection statements
    | IF OPEN_PARENS expression CLOSE_PARENS if_body (ELSE if_body)?                    # ifStatement
    | SWITCH OPEN_PARENS expression CLOSE_PARENS OPEN_BRACE switch_section* CLOSE_BRACE # switchStatement

    // iteration statements
    | WHILE OPEN_PARENS expression CLOSE_PARENS embedded_statement                                            # whileStatement
    | DO embedded_statement WHILE OPEN_PARENS expression CLOSE_PARENS ';'                                     # doStatement
    | FOR OPEN_PARENS for_initializer? ';' expression? ';' for_iterator? CLOSE_PARENS embedded_statement      # forStatement
    | AWAIT? FOREACH OPEN_PARENS local_variable_type identifier IN expression CLOSE_PARENS embedded_statement # foreachStatement

    // jump statements
    | BREAK ';'                                                              # breakStatement
    | CONTINUE ';'                                                           # continueStatement
    | GOTO (identifier | CASE expression | DEFAULT) ';'                      # gotoStatement
    | RETURN expression? ';'                                                 # returnStatement
    | THROW expression? ';'                                                  # throwStatement
    | TRY block (catch_clauses finally_clause? | finally_clause)             # tryStatement
    | CHECKED block                                                          # checkedStatement
    | UNCHECKED block                                                        # uncheckedStatement
    | LOCK OPEN_PARENS expression CLOSE_PARENS embedded_statement            # lockStatement
    | USING OPEN_PARENS resource_acquisition CLOSE_PARENS embedded_statement # usingStatement
    | YIELD (RETURN expression | BREAK) ';'                                  # yieldStatement

    // unsafe statements
    | UNSAFE block                                                                             # unsafeStatement
    | FIXED OPEN_PARENS pointer_type fixed_pointer_declarators CLOSE_PARENS embedded_statement # fixedStatement
    ;

block
    : OPEN_BRACE statement_list? CLOSE_BRACE
    ;

local_variable_declaration
    : (USING | REF | REF READONLY)? local_variable_type local_variable_declarator (
        ',' local_variable_declarator { this.IsLocalVariableDeclaration() }?
    )*
    | FIXED pointer_type fixed_pointer_declarators
    ;

local_variable_type
    : VAR
    | type_
    ;

local_variable_declarator
    : identifier ('=' REF? local_variable_initializer)?
    ;

local_variable_initializer
    : expression
    | array_initializer
    | stackalloc_initializer
    ;

local_constant_declaration
    : CONST type_ constant_declarators
    ;

if_body
    : block
    | simple_embedded_statement
    ;

ref_local_function_body
    : block
    | '=>' 'ref' variable_reference ';'
    ;

// Source: §13.7 Expression statements
expression_statement
    : statement_expression ';'
    ;

statement_expression
    : null_conditional_invocation_expression
    | invocation_expression
    | object_creation_expression
    | assignment
    | post_increment_expression
    | post_decrement_expression
    | pre_increment_expression
    | pre_decrement_expression
    | await_expression
    ;

// Source: §13.8.1 General
selection_statement
    : if_statement
    | switch_statement
    ;

// Source: §13.8.2 The if statement
if_statement
    : 'if' '(' boolean_expression ')' embedded_statement
    | 'if' '(' boolean_expression ')' embedded_statement
      'else' embedded_statement
    ;

// Source: §13.8.3 The switch statement
switch_statement
    : 'switch' '(' expression ')' switch_block
    ;

switch_block
    : '{' switch_section* '}'
    ;

switch_section
    : switch_label+ statement_list
    ;

switch_label
    : CASE expression case_guard? ':'
    | DEFAULT ':'
    ;

case_guard
    : WHEN expression
    ;

statement_list
    : statement+
    ;

for_initializer
    : local_variable_declaration
    | expression (',' expression)*
    ;

for_iterator
    : expression (',' expression)*
    ;

catch_clauses
    : specific_catch_clause specific_catch_clause* general_catch_clause?
    | general_catch_clause
    ;

specific_catch_clause
    : CATCH OPEN_PARENS class_type identifier? CLOSE_PARENS exception_filter? block
    ;

general_catch_clause
    : CATCH exception_filter? block
    ;

exception_filter // C# 6
    : WHEN OPEN_PARENS expression CLOSE_PARENS
    ;

finally_clause
    : FINALLY block
    ;

resource_acquisition
    : local_variable_declaration
    | expression
    ;

//B.2.6 Namespaces;
namespace_declaration
    : NAMESPACE qi = qualified_identifier namespace_body ';'?
    ;

qualified_identifier
    : identifier ('.' identifier)*
    ;

namespace_body
    : OPEN_BRACE extern_alias_directives? using_directives? namespace_member_declarations? CLOSE_BRACE
    ;

extern_alias_directives
    : extern_alias_directive+
    ;

extern_alias_directive
    : EXTERN ALIAS identifier ';'
    ;

using_directives
    : using_directive+
    ;

using_directive
    : USING identifier '=' namespace_or_type_name ';' # usingAliasDirective
    | USING namespace_or_type_name ';'                # usingNamespaceDirective
    // C# 6: https://msdn.microsoft.com/en-us/library/ms228593.aspx
    | USING STATIC namespace_or_type_name ';' # usingStaticDirective
    | USING identifier '=' tuple_type ';' #usingTupleTypeDefinition
    ;

namespace_member_declarations
    : namespace_member_declaration+
    ;

namespace_member_declaration
    : namespace_declaration
    | type_declaration
    ;

type_declaration
    : attributes? all_member_modifiers? (
        class_definition
        | struct_definition
        | interface_definition
        | enum_definition
        | delegate_definition
    )
    ;

qualified_alias_member
    : identifier '::' identifier type_argument_list?
    ;

//B.2.7 Classes;
type_parameter_list
    : '<' type_parameter (',' type_parameter)* '>'
    ;

type_parameter
    : attributes? identifier
    ;

class_base
    : ':' class_type (',' namespace_or_type_name)*
    ;

interface_type_list
    : namespace_or_type_name (',' namespace_or_type_name)*
    ;

type_parameter_constraints_clauses
    : type_parameter_constraints_clause+
    ;

type_parameter_constraints_clause
    : WHERE identifier ':' type_parameter_constraints
    ;

type_parameter_constraints
    : constructor_constraint
    | primary_constraint (',' secondary_constraints)? (',' constructor_constraint)?
    ;

primary_constraint
    : class_type
    | CLASS '?'?
    | STRUCT
    | UNMANAGED
    | NOTNULL
    ;

// namespace_or_type_name includes identifier
secondary_constraints
    : namespace_or_type_name (',' namespace_or_type_name)*
    ;

constructor_constraint
    : NEW OPEN_PARENS CLOSE_PARENS
    ;

class_body
    : OPEN_BRACE class_member_declarations? CLOSE_BRACE
    ;

class_member_declarations
    : class_member_declaration+
    ;

class_member_declaration
    : attributes? all_member_modifiers? (common_member_declaration | destructor_definition)
    ;

all_member_modifiers
    : all_member_modifier+
    ;

all_member_modifier
    : NEW
    | PUBLIC
    | PROTECTED
    | INTERNAL
    | PRIVATE
    | READONLY
    | VOLATILE
    | VIRTUAL
    | SEALED
    | OVERRIDE
    | ABSTRACT
    | STATIC
    | UNSAFE
    | EXTERN
    | PARTIAL
    | ASYNC // C# 5
    ;

// represents the intersection of struct_member_declaration and class_member_declaration
common_member_declaration
    : constant_declaration
    | typed_member_declaration
    | event_declaration
    | conversion_operator_declarator (body | right_arrow throwable_expression ';') // C# 6
    | constructor_declaration
    | VOID method_declaration
    | class_definition
    | struct_definition
    | interface_definition
    | enum_definition
    | delegate_definition
    ;

typed_member_declaration
    : (REF | READONLY REF | REF READONLY)? type_ (
        namespace_or_type_name '.' indexer_declaration
        | method_declaration
        | property_declaration
        | indexer_declaration
        | operator_declaration
        | field_declaration
    )
    ;

constant_declarators
    : constant_declarator (',' constant_declarator)*
    ;

constant_declarator
    : identifier '=' expression
    ;

variable_declarators
    : variable_declarator (',' variable_declarator)*
    ;

variable_declarator
    : identifier ('=' variable_initializer)?
    ;

variable_initializer
    : expression
    | array_initializer
    ;

return_type
    : type_
    | VOID
    ;

member_name
    : namespace_or_type_name
    ;

method_body
    : block
    | ';'
    ;

formal_parameter_list
    : parameter_array
    | fixed_parameters (',' parameter_array)?
    ;

fixed_parameters
    : fixed_parameter (',' fixed_parameter)*
    ;

fixed_parameter
    : attributes? parameter_modifier? arg_declaration
    | ARGLIST
    ;

parameter_modifier
    : REF
    | OUT
    | IN
    | REF THIS
    | IN THIS
    | THIS
    ;

parameter_array
    : attributes? PARAMS array_type identifier
    ;

accessor_declarations
    : attrs = attributes? mods = accessor_modifier? (
        GET accessor_body set_accessor_declaration?
        | SET accessor_body get_accessor_declaration?
    )
    ;

get_accessor_declaration
    : attributes? accessor_modifier? GET accessor_body
    ;

set_accessor_declaration
    : attributes? accessor_modifier? SET accessor_body
    ;

accessor_modifier
    : PROTECTED
    | INTERNAL
    | PRIVATE
    | PROTECTED INTERNAL
    | INTERNAL PROTECTED
    ;

accessor_body
    : block
    | ';'
    ;

event_accessor_declarations
    : attributes? (ADD block remove_accessor_declaration | REMOVE block add_accessor_declaration)
    ;

add_accessor_declaration
    : attributes? ADD block
    ;

remove_accessor_declaration
    : attributes? REMOVE block
    ;

overloadable_operator
    : '+'
    | '-'
    | BANG
    | '~'
    | '++'
    | '--'
    | TRUE
    | FALSE
    | '*'
    | '/'
    | '%'
    | '&'
    | '|'
    | '^'
    | '<<'
    | right_shift
    | OP_EQ
    | OP_NE
    | '>'
    | '<'
    | '>='
    | '<='
    ;

conversion_operator_declarator
    : (IMPLICIT | EXPLICIT) OPERATOR type_ OPEN_PARENS arg_declaration CLOSE_PARENS
    ;

constructor_initializer
    : ':' (BASE | THIS) OPEN_PARENS argument_list? CLOSE_PARENS
    ;

body
    : block
    | ';'
    ;

//B.2.8 Structs
struct_interfaces
    : ':' interface_type_list
    ;

struct_body
    : OPEN_BRACE struct_member_declaration* CLOSE_BRACE
    ;

struct_member_declaration
    : attributes? all_member_modifiers? (
        common_member_declaration
        | FIXED type_ fixed_size_buffer_declarator+ ';'
    )
    ;

//B.2.9 Arrays
array_type
    : base_type (('*' | '?')* rank_specifier)+
    ;

rank_specifier
    : '[' ','* ']'
    ;

array_initializer
    : OPEN_BRACE (variable_initializer (',' variable_initializer)* ','?)? CLOSE_BRACE
    ;

//B.2.10 Interfaces
variant_type_parameter_list
    : '<' variant_type_parameter (',' variant_type_parameter)* '>'
    ;

variant_type_parameter
    : attributes? variance_annotation? identifier
    ;

variance_annotation
    : IN
    | OUT
    ;

interface_base
    : ':' interface_type_list
    ;

interface_body // ignored in csharp 8
    : OPEN_BRACE interface_member_declaration* CLOSE_BRACE
    ;

interface_member_declaration
    : attributes? NEW? (
        UNSAFE? (REF | REF READONLY | READONLY REF)? type_ (
            identifier type_parameter_list? OPEN_PARENS formal_parameter_list? CLOSE_PARENS type_parameter_constraints_clauses? ';'
            | identifier OPEN_BRACE interface_accessors CLOSE_BRACE
            | THIS '[' formal_parameter_list ']' OPEN_BRACE interface_accessors CLOSE_BRACE
        )
        | UNSAFE? VOID identifier type_parameter_list? OPEN_PARENS formal_parameter_list? CLOSE_PARENS type_parameter_constraints_clauses? ';'
        | EVENT type_ identifier ';'
    )
    ;

interface_accessors
    : attributes? (GET ';' (attributes? SET ';')? | SET ';' (attributes? GET ';')?)
    ;

//B.2.11 Enums
enum_base
    : ':' type_
    ;

enum_body
    : OPEN_BRACE (enum_member_declaration (',' enum_member_declaration)* ','?)? CLOSE_BRACE
    ;

enum_member_declaration
    : attributes? identifier ('=' expression)?
    ;

//B.2.12 Delegates

//B.2.13 Attributes
global_attribute_section
    : '[' global_attribute_target ':' attribute_list ','? ']'
    ;

global_attribute_target
    : keyword
    | identifier
    ;

attributes
    : attribute_section+
    ;

attribute_section
    : '[' (attribute_target ':')? attribute_list ','? ']'
    ;

attribute_target
    : keyword
    | identifier
    ;

attribute_list
    : attribute (',' attribute)*
    ;

attribute
    : attribute_name attribute_arguments?
    ;

attribute_name
    : type_name
    ;

attribute_arguments
    : '(' positional_argument_list? ')'
    | '(' positional_argument_list ',' named_argument_list ')'
    | '(' named_argument_list ')'
    ;

positional_argument_list
    : positional_argument (',' positional_argument)*
    ;

positional_argument
    : argument_name? attribute_argument_expression
    ;

named_argument_list
    : named_argument (','  named_argument)*
    ;

named_argument
    : identifier '=' attribute_argument_expression
    ;

attribute_argument_expression
    : expression
    ;

// ###################################################################################
// Lexer
// ###################################################################################


// Source: §6.3.1 General
DEFAULT  : 'default' ;
NULL     : 'null' ;
TRUE     : 'true' ;
FALSE    : 'false' ;
ASTERISK : '*' ;
SLASH    : '/' ;

// Source: §6.3.1 General
input
    : input_section?
    ;

input_section
    : input_section_part+
    ;

input_section_part
    : input_element* New_Line
    | PP_Directive
    ;

input_element
    : Whitespace
    | Comment
    | token
    ;

// Source: §6.3.2 Line terminators
New_Line
    : New_Line_Character
    | '\u000D\u000A'    // carriage return, line feed 
    ;

// Source: §6.3.3 Comments
Comment
    : Single_Line_Comment
    | Delimited_Comment
    ;

fragment Single_Line_Comment
    : '//' Input_Character*
    ;

fragment Input_Character
    // anything but New_Line_Character
    : ~('\u000D' | '\u000A'   | '\u0085' | '\u2028' | '\u2029')
    ;
    
fragment New_Line_Character
    : '\u000D'  // carriage return
    | '\u000A'  // line feed
    | '\u0085'  // next line
    | '\u2028'  // line separator
    | '\u2029'  // paragraph separator
    ;
    
fragment Delimited_Comment
    : '/*' Delimited_Comment_Section* ASTERISK+ '/'
    ;
    
fragment Delimited_Comment_Section
    : SLASH
    | ASTERISK* Not_Slash_Or_Asterisk
    ;

fragment Not_Slash_Or_Asterisk
    : ~('/' | '*')    // Any except SLASH or ASTERISK
    ;

// Source: §6.3.4 White space
Whitespace
    : [\p{Zs}]  // any character with Unicode class Zs
    | '\u0009'  // horizontal tab
    | '\u000B'  // vertical tab
    | '\u000C'  // form feed
    ;

// Source: §6.4.1 General
token
    : identifier
    | keyword
    | Integer_Literal
    | Real_Literal
    | Character_Literal
    | String_Literal
    | operator_or_punctuator
    ;

// Source: §6.4.2 Unicode character escape sequences
fragment Unicode_Escape_Sequence
    : '\\u' Hex_Digit Hex_Digit Hex_Digit Hex_Digit
    | '\\U' Hex_Digit Hex_Digit Hex_Digit Hex_Digit
            Hex_Digit Hex_Digit Hex_Digit Hex_Digit
    ;

// Source: §6.4.3 Identifiers
identifier
    : Simple_Identifier
    | contextual_keyword
    ;

Simple_Identifier
    : Available_Identifier
    | Escaped_Identifier
    ;

fragment Available_Identifier
    // excluding keywords or contextual keywords, see note below
    : Basic_Identifier
    ;

fragment Escaped_Identifier
    // Includes keywords and contextual keywords prefixed by '@'.
    // See note below.
    : '@' Basic_Identifier 
    ;

fragment Basic_Identifier
    : Identifier_Start_Character Identifier_Part_Character*
    ;

attribute_argument
    : (identifier ':')? expression
    ;

//B.3 Grammar extensions for unsafe code
pointer_type
    : (simple_type | class_type) (rank_specifier | '?')* '*'
    | VOID '*'
    ;

fixed_pointer_declarators
    : fixed_pointer_declarator (',' fixed_pointer_declarator)*
    ;

fragment Letter_Character
    // Category Letter, all subcategories; category Number, subcategory letter.
    : [\p{L}\p{Nl}]
    // Only escapes for categories L & Nl allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Combining_Character
    // Category Mark, subcategories non-spacing and spacing combining.
    : [\p{Mn}\p{Mc}]
    // Only escapes for categories Mn & Mc allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Decimal_Digit_Character
    // Category Number, subcategory decimal digit.
    : [\p{Nd}]
    // Only escapes for category Nd allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Connecting_Character
    // Category Punctuation, subcategory connector.
    : [\p{Pc}]
    // Only escapes for category Pc allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Formatting_Character
    // Category Other, subcategory format.
    : [\p{Cf}]
    // Only escapes for category Cf allowed, see note below.
    | Unicode_Escape_Sequence
    ;

// Source: §6.4.4 Keywords
keyword
    : 'abstract' | 'as'       | 'base'       | 'bool'      | 'break'
    | 'byte'     | 'case'     | 'catch'      | 'char'      | 'checked'
    | 'class'    | 'const'    | 'continue'   | 'decimal'   | DEFAULT
    | 'delegate' | 'do'       | 'double'     | 'else'      | 'enum'
    | 'event'    | 'explicit' | 'extern'     | FALSE       | 'finally'
    | 'fixed'    | 'float'    | 'for'        | 'foreach'   | 'goto'
    | 'if'       | 'implicit' | 'in'         | 'int'       | 'interface'
    | 'internal' | 'is'       | 'lock'       | 'long'      | 'namespace'
    | 'new'      | NULL       | 'object'     | 'operator'  | 'out'
    | 'override' | 'params'   | 'private'    | 'protected' | 'public'
    | 'readonly' | 'ref'      | 'return'     | 'sbyte'     | 'sealed'
    | 'short'    | 'sizeof'   | 'stackalloc' | 'static'    | 'string'
    | 'struct'   | 'switch'   | 'this'       | 'throw'     | TRUE
    | 'try'      | 'typeof'   | 'uint'       | 'ulong'     | 'unchecked'
    | 'unsafe'   | 'ushort'   | 'using'      | 'virtual'   | 'void'
    | 'volatile' | 'while'
    ;

// Source: §6.4.4 Keywords
contextual_keyword
    : 'add'    | 'alias'      | 'ascending' | 'async'     | 'await'
    | 'by'     | 'descending' | 'dynamic'   | 'equals'    | 'from'
    | 'get'    | 'global'     | 'group'     | 'into'      | 'join'
    | 'let'    | 'nameof'     | 'on'        | 'orderby'   | 'partial'
    | 'remove' | 'select'     | 'set'       | 'unmanaged' | 'value'
    | 'var'    | 'when'       | 'where'     | 'yield'
    ;

// Source: §6.4.5.1 General
literal
    : boolean_literal
    | Integer_Literal
    | Real_Literal
    | Character_Literal
    | String_Literal
    | null_literal
    ;

// Source: §6.4.5.2 Boolean literals
boolean_literal
    : TRUE
    | FALSE
    ;

// Source: §6.4.5.3 Integer literals
Integer_Literal
    : Decimal_Integer_Literal
    | Hexadecimal_Integer_Literal
    | Binary_Integer_Literal
    ;

fragment Decimal_Integer_Literal
    : Decimal_Digit Decorated_Decimal_Digit* Integer_Type_Suffix?
    ;

fragment Decorated_Decimal_Digit
    : '_'* Decimal_Digit
    ;
       
fragment Decimal_Digit
    : '0'..'9'
    ;
    
fragment Integer_Type_Suffix
    : 'U' | 'u' | 'L' | 'l' |
      'UL' | 'Ul' | 'uL' | 'ul' | 'LU' | 'Lu' | 'lU' | 'lu'
    ;
    
fragment Hexadecimal_Integer_Literal
    : ('0x' | '0X') Decorated_Hex_Digit+ Integer_Type_Suffix?
    ;

fragment Decorated_Hex_Digit
    : '_'* Hex_Digit
    ;
       
fragment Hex_Digit
    : '0'..'9' | 'A'..'F' | 'a'..'f'
    ;
   
fragment Binary_Integer_Literal
    : ('0b' | '0B') Decorated_Binary_Digit+ Integer_Type_Suffix?
    ;

fragment Decorated_Binary_Digit
    : '_'* Binary_Digit
    ;
       
fragment Binary_Digit
    : '0' | '1'
    ;

// Source: §6.4.5.4 Real literals
Real_Literal
    : Decimal_Digit Decorated_Decimal_Digit* '.'
      Decimal_Digit Decorated_Decimal_Digit* Exponent_Part? Real_Type_Suffix?
    | '.' Decimal_Digit Decorated_Decimal_Digit* Exponent_Part? Real_Type_Suffix?
    | Decimal_Digit Decorated_Decimal_Digit* Exponent_Part Real_Type_Suffix?
    | Decimal_Digit Decorated_Decimal_Digit* Real_Type_Suffix
    ;

fragment Exponent_Part
    : ('e' | 'E') Sign? Decimal_Digit Decorated_Decimal_Digit*
    ;

fragment Sign
    : '+' | '-'
    ;

fragment Real_Type_Suffix
    : 'F' | 'f' | 'D' | 'd' | 'M' | 'm'
    ;

// Source: §6.4.5.5 Character literals
Character_Literal
    : '\'' Character '\''
    ;
    
fragment Character
    : Single_Character
    | Simple_Escape_Sequence
    | Hexadecimal_Escape_Sequence
    | Unicode_Escape_Sequence
    ;
    
fragment Single_Character
    // anything but ', \, and New_Line_Character
    : ~['\\\u000D\u000A\u0085\u2028\u2029]
    ;
    
fragment Simple_Escape_Sequence
    : '\\\'' | '\\"' | '\\\\' | '\\0' | '\\a' | '\\b' |
      '\\f' | '\\n' | '\\r' | '\\t' | '\\v'
    ;
    
fragment Hexadecimal_Escape_Sequence
    : '\\x' Hex_Digit Hex_Digit? Hex_Digit? Hex_Digit?
    ;

// Source: §6.4.5.6 String literals
String_Literal
    : Regular_String_Literal
    | Verbatim_String_Literal
    ;
    
fragment Regular_String_Literal
    : '"' Regular_String_Literal_Character* '"'
    ;
    
fragment Regular_String_Literal_Character
    : Single_Regular_String_Literal_Character
    | Simple_Escape_Sequence
    | Hexadecimal_Escape_Sequence
    | Unicode_Escape_Sequence
    ;

fragment Single_Regular_String_Literal_Character
    // anything but ", \, and New_Line_Character
    : ~["\\\u000D\u000A\u0085\u2028\u2029]
    ;

fragment Verbatim_String_Literal
    : '@"' Verbatim_String_Literal_Character* '"'
    ;
    
fragment Verbatim_String_Literal_Character
    : Single_Verbatim_String_Literal_Character
    | Quote_Escape_Sequence
    ;
    
fragment Single_Verbatim_String_Literal_Character
    : ~["]     // anything but quotation mark (U+0022)
    ;
    
fragment Quote_Escape_Sequence
    : '""'
    ;

// Source: §6.4.5.7 The null literal
null_literal
    : NULL
    ;

// Source: §6.4.6 Operators and punctuators
operator_or_punctuator
    : '{'  | '}'  | '['  | ']'  | '('   | ')'  | '.'  | ','  | ':'  | ';'
    | '+'  | '-'  | ASTERISK    | SLASH | '%'  | '&'  | '|'  | '^'  | '!' | '~'
    | '='  | '<'  | '>'  | '?'  | '??'  | '::' | '++' | '--' | '&&' | '||'
    | '->' | '==' | '!=' | '<=' | '>='  | '+=' | '-=' | '*=' | '/=' | '%='
    | '&=' | '|=' | '^=' | '<<' | '<<=' | '=>'
    ;

right_shift
    : '>'  '>'
    ;

right_shift_assignment
    : '>' '>='
    ;

// Source: §6.5.1 General
PP_Directive
    : PP_Start PP_Kind PP_New_Line
    ;

fragment PP_Kind
    : PP_Declaration
    | PP_Conditional
    | PP_Line
    | PP_Diagnostic
    | PP_Region
    | PP_Pragma
    ;

// Only recognised at the beginning of a line
fragment PP_Start
    // See note below.
    : { getCharPositionInLine() == 0 }? PP_Whitespace? '#' PP_Whitespace?
    ;

fragment PP_Whitespace
    : ( [\p{Zs}]  // any character with Unicode class Zs
      | '\u0009'  // horizontal tab
      | '\u000B'  // vertical tab
      | '\u000C'  // form feed
      )+
    ;

fragment PP_New_Line
    : PP_Whitespace? Single_Line_Comment? New_Line
    ;

// Source: §6.5.2 Conditional compilation symbols
fragment PP_Conditional_Symbol
    // Must not be equal to tokens TRUE or FALSE. See note below.
    : Basic_Identifier
    ;

// Source: §6.5.3 Pre-processing expressions
fragment PP_Expression
    : PP_Whitespace? PP_Or_Expression PP_Whitespace?
    ;
    
fragment PP_Or_Expression
    : PP_And_Expression (PP_Whitespace? '||' PP_Whitespace? PP_And_Expression)*
    ;
    
fragment PP_And_Expression
    : PP_Equality_Expression (PP_Whitespace? '&&' PP_Whitespace?
      PP_Equality_Expression)*
    ;

fragment PP_Equality_Expression
    : PP_Unary_Expression (PP_Whitespace? ('==' | '!=') PP_Whitespace?
      PP_Unary_Expression)*
    ;
    
fragment PP_Unary_Expression
    : PP_Primary_Expression
    | '!' PP_Whitespace? PP_Unary_Expression
    ;
    
fragment PP_Primary_Expression
    : TRUE
    | FALSE
    | PP_Conditional_Symbol
    | '(' PP_Whitespace? PP_Expression PP_Whitespace? ')'
    ;

// Source: §6.5.4 Definition directives
fragment PP_Declaration
    : 'define' PP_Whitespace PP_Conditional_Symbol
    | 'undef' PP_Whitespace PP_Conditional_Symbol
    ;

// Source: §6.5.5 Conditional compilation directives
fragment PP_Conditional
    : PP_If_Section
    | PP_Elif_Section
    | PP_Else_Section
    | PP_Endif
    ;

fragment PP_If_Section
    : 'if' PP_Whitespace PP_Expression
    ;
    
fragment PP_Elif_Section
    : 'elif' PP_Whitespace PP_Expression
    ;
    
fragment PP_Else_Section
    : 'else'
    ;
    
fragment PP_Endif
    : 'endif'
    ;

string_literal
    : interpolated_regular_string
    | interpolated_verbatium_string
    | REGULAR_STRING
    | VERBATIUM_STRING
    ;

interpolated_regular_string
    : INTERPOLATED_REGULAR_STRING_START interpolated_regular_string_part* DOUBLE_QUOTE_INSIDE
    ;

interpolated_verbatium_string
    : INTERPOLATED_VERBATIUM_STRING_START interpolated_verbatium_string_part* DOUBLE_QUOTE_INSIDE
    ;

interpolated_regular_string_part
    : interpolated_string_expression
    | DOUBLE_CURLY_INSIDE
    | REGULAR_CHAR_INSIDE
    | REGULAR_STRING_INSIDE
    ;

interpolated_verbatium_string_part
    : interpolated_string_expression
    | DOUBLE_CURLY_INSIDE
    | VERBATIUM_DOUBLE_QUOTE_INSIDE
    | VERBATIUM_INSIDE_STRING
    ;

interpolated_string_expression
    : expression (',' expression)* (':' FORMAT_STRING+)?
    ;

//B.1.7 Keywords
keyword
    : ABSTRACT
    | AND   // C# 9 patterns
    | AS
    | BASE
    | BOOL
    | BREAK
    | BYTE
    | CASE
    | CATCH
    | CHAR
    | CHECKED
    | CLASS
    | CONST
    | CONTINUE
    | DECIMAL
    | DEFAULT
    | DELEGATE
    | DO
    | DOUBLE
    | ELSE
    | ENUM
    | EVENT
    | EXPLICIT
    | EXTERN
    | FALSE
    | FINALLY
    | FIXED
    | FLOAT
    | FOR
    | FOREACH
    | GOTO
    | IF
    | IMPLICIT
    | IN
    | INT
    | INTERFACE
    | INTERNAL
    | IS
    | LOCK
    | LONG
    | NAMESPACE
    | NEW
    | NOTNULL
    | NOT    // C# 9 patterns
    | NULL_
    | OBJECT
    | OPERATOR
    | OR     // C# 9 patterns
    | OUT
    | OVERRIDE
    | PARAMS
    | PRIVATE
    | PROTECTED
    | PUBLIC
    | READONLY
    | REF
    | RETURN
    | SBYTE
    | SEALED
    | SHORT
    | SIZEOF
    | STACKALLOC
    | STATIC
    | STRING
    | STRUCT
    | SWITCH
    | THIS
    | THROW
    | TRUE
    | TRY
    | TYPEOF
    | UINT
    | ULONG
    | UNCHECKED
    | UNMANAGED
    | UNSAFE
    | USHORT
    | USING
    | VIRTUAL
    | VOID
    | VOLATILE
    | WHILE
    ;

// -------------------- extra rules for modularization --------------------------------

class_definition
    : (RECORD CLASS? | CLASS) identifier type_parameter_list? class_base? type_parameter_constraints_clauses? class_body ';'?
    ;

struct_definition
    : (READONLY RECORD? | REF | RECORD)?  STRUCT identifier type_parameter_list? struct_interfaces? type_parameter_constraints_clauses? struct_body ';'?
    ;

interface_definition
    : INTERFACE identifier variant_type_parameter_list? interface_base? type_parameter_constraints_clauses? class_body ';'?
    ;

// ###################################################################################
// Unsafe context
// ###################################################################################

// Source: §23.2 Unsafe contexts
unsafe_modifier
    : 'unsafe'
    ;

unsafe_statement
    : 'unsafe' block
    ;

// Source: §23.3 Pointer types
pointer_type
    : value_type ('*')+
    | 'void' ('*')+
    ;

// Source: §23.6.2 Pointer indirection
pointer_indirection_expression
    : '*' unary_expression
    ;

// Source: §23.6.3 Pointer member access
pointer_member_access
    : primary_expression '->' identifier type_argument_list?
    ;

// Source: §23.6.4 Pointer element access
pointer_element_access
    : primary_no_array_creation_expression '[' expression ']'
    ;

// Source: §23.6.5 The address-of operator
addressof_expression
    : '&' unary_expression
    ;

// Source: §23.7 The fixed statement
fixed_statement
    : 'fixed' '(' pointer_type fixed_pointer_declarators ')' embedded_statement
    ;

fixed_pointer_declarators
    : fixed_pointer_declarator (','  fixed_pointer_declarator)*
    ;

fixed_pointer_declarator
    : identifier '=' fixed_pointer_initializer
    ;

fixed_pointer_initializer
    : '&' variable_reference
    | expression
    ;

// Source: §23.8.2 Fixed-size buffer declarations
fixed_size_buffer_declaration
    : attributes? fixed_size_buffer_modifier* 'fixed' buffer_element_type
      fixed_size_buffer_declarators ';'
    ;

fixed_size_buffer_modifier
    : 'new'
    | 'public'
    | 'internal'
    | 'private'
    | 'unsafe'
    ;

buffer_element_type
    : type
    ;

fixed_size_buffer_declarators
    : fixed_size_buffer_declarator (',' fixed_size_buffer_declarator)*
    ;

fixed_size_buffer_declarator
    : identifier '[' constant_expression ']'
    ;