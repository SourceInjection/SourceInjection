grammar CSharpParser;
@parser::header {#pragma warning disable 3021}

// Source: §7.8.1 General
namespace_name
    : namespace_or_type_name
    ;

type_name
    : namespace_or_type_name
    ;
    
namespace_or_type_name
    : identifier type_argument_list?
    | namespace_or_type_name '.' identifier type_argument_list?
    | qualified_alias_member
    ;

// Source: §8.1 General
type
    : reference_type
    | value_type
    | type_parameter
    | pointer_type     // unsafe code support
    ;

// Source: §8.2.1 General
reference_type
    : class_type
    | interface_type
    | array_type
    | delegate_type
    | 'dynamic'
    ;

class_type
    : type_name
    | 'object'
    | 'string'
    ;

interface_type
    : type_name
    ;

array_type
    : non_array_type rank_specifier+
    ;

non_array_type
    : value_type
    | class_type
    | interface_type
    | delegate_type
    | 'dynamic'
    | type_parameter
    | pointer_type      // unsafe code support
    ;

rank_specifier
    : '[' ','* ']'
    ;

delegate_type
    : type_name
    ;

// Source: §8.3.1 General
value_type
    : non_nullable_value_type
    | nullable_value_type
    ;

non_nullable_value_type
    : struct_type
    | enum_type
    ;

struct_type
    : type_name
    | simple_type
    | tuple_type
    ;

simple_type
    : numeric_type
    | 'bool'
    ;

numeric_type
    : integral_type
    | floating_point_type
    | 'decimal'
    ;

integral_type
    : 'sbyte'
    | 'byte'
    | 'short'
    | 'ushort'
    | 'int'
    | 'uint'
    | 'long'
    | 'ulong'
    | 'char'
    ;

floating_point_type
    : 'float'
    | 'double'
    ;

tuple_type
    : '(' tuple_type_element (',' tuple_type_element)+ ')'
    ;
    
tuple_type_element
    : type identifier?
    ;
    
enum_type
    : type_name
    ;

nullable_value_type
    : non_nullable_value_type '?'
    ;

// Source: §8.4.2 Type arguments
type_argument_list
    : '<' type_arguments '>'
    ;

type_arguments
    : type_argument (',' type_argument)*
    ;   

type_argument
    : type
    ;

// Source: §8.5 Type parameters
type_parameter
    : identifier
    ;

// Source: §8.8 Unmanaged types
unmanaged_type
    : value_type
    | pointer_type     // unsafe code support
    ;

// Source: §9.5 Variable references
variable_reference
    : expression
    ;

// Source: §11.2.1 General
pattern
    : declaration_pattern
    | constant_pattern
    | var_pattern
    ;

// Source: §11.2.2 Declaration pattern
declaration_pattern
    : type simple_designation
    ;
simple_designation
    : single_variable_designation
    ;
single_variable_designation
    : identifier
    ;

// Source: §11.2.3 Constant pattern
constant_pattern
    : constant_expression
    ;

// Source: §11.2.4 Var pattern
var_pattern
    : 'var' designation
    ;
designation
    : simple_designation
    ;

// Source: §12.6.2.1 General
argument_list
    : argument (',' argument)*
    ;

argument
    : argument_name? argument_value
    ;

argument_name
    : identifier ':'
    ;

argument_value
    : expression
    | 'in' variable_reference
    | 'ref' variable_reference
    | 'out' variable_reference
    ;

// Source: §12.8.1 General
primary_expression
    : primary_no_array_creation_expression
    | array_creation_expression
    ;

primary_no_array_creation_expression
    : literal
    | interpolated_string_expression
    | simple_name
    | parenthesized_expression
    | tuple_expression
    | member_access
    | null_conditional_member_access
    | invocation_expression
    | element_access
    | null_conditional_element_access
    | this_access
    | base_access
    | post_increment_expression
    | post_decrement_expression
    | object_creation_expression
    | delegate_creation_expression
    | anonymous_object_creation_expression
    | typeof_expression
    | sizeof_expression
    | checked_expression
    | unchecked_expression
    | default_value_expression
    | nameof_expression    
    | anonymous_method_expression
    | pointer_member_access     // unsafe code support
    | pointer_element_access    // unsafe code support
    | stackalloc_expression
    ;

// Source: §12.8.3 Interpolated string expressions
interpolated_string_expression
    : interpolated_regular_string_expression
    | interpolated_verbatim_string_expression
    ;

// interpolated regular string expressions

interpolated_regular_string_expression
    : Interpolated_Regular_String_Start Interpolated_Regular_String_Mid?
      ('{' regular_interpolation '}' Interpolated_Regular_String_Mid?)*
      Interpolated_Regular_String_End
    ;

regular_interpolation
    : expression (',' interpolation_minimum_width)?
      Regular_Interpolation_Format?
    ;

interpolation_minimum_width
    : constant_expression
    ;

// interpolated verbatim string expressions

interpolated_verbatim_string_expression
    : Interpolated_Verbatim_String_Start Interpolated_Verbatim_String_Mid?
      ('{' verbatim_interpolation '}' Interpolated_Verbatim_String_Mid?)*
      Interpolated_Verbatim_String_End
    ;

verbatim_interpolation
    : expression (',' interpolation_minimum_width)?
      Verbatim_Interpolation_Format?
    ;

// Source: §12.8.4 Simple names
simple_name
    : identifier type_argument_list?
    ;

// Source: §12.8.5 Parenthesized expressions
parenthesized_expression
    : '(' expression ')'
    ;

// Source: §12.8.6 Tuple expressions
tuple_expression
    : '(' tuple_element (',' tuple_element)+ ')'
    | deconstruction_expression
    ;
    
tuple_element
    : (identifier ':')? expression
    ;
    
deconstruction_expression
    : 'var' deconstruction_tuple
    ;
    
deconstruction_tuple
    : '(' deconstruction_element (',' deconstruction_element)+ ')'
    ;

deconstruction_element
    : deconstruction_tuple
    | identifier
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

null_conditional_projection_initializer
    : primary_expression '?' '.' identifier type_argument_list?
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

// Source: §12.8.12 Null Conditional Element Access
null_conditional_element_access
    : primary_no_array_creation_expression '?' '[' argument_list ']'
      dependent_access*
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

// Source: §12.8.16.3 Object initializers
object_initializer
    : '{' member_initializer_list? '}'
    | '{' member_initializer_list ',' '}'
    ;

member_initializer_list
    : member_initializer (',' member_initializer)*
    ;

member_initializer
    : initializer_target '=' initializer_value
    ;

initializer_target
    : identifier
    | '[' argument_list ']'
    ;

initializer_value
    : expression
    | object_or_collection_initializer
    ;

// Source: §12.8.16.4 Collection initializers
collection_initializer
    : '{' element_initializer_list '}'
    | '{' element_initializer_list ',' '}'
    ;

element_initializer_list
    : element_initializer (',' element_initializer)*
    ;

element_initializer
    : non_assignment_expression
    | '{' expression_list '}'
    ;

expression_list
    : expression
    | expression_list ',' expression
    ;

// Source: §12.8.16.5 Array creation expressions
array_creation_expression
    : 'new' non_array_type '[' expression_list ']' rank_specifier*
      array_initializer?
    | 'new' array_type array_initializer
    | 'new' rank_specifier array_initializer
    ;

// Source: §12.8.16.6 Delegate creation expressions
delegate_creation_expression
    : 'new' delegate_type '(' expression ')'
    ;

// Source: §12.8.16.7 Anonymous object creation expressions
anonymous_object_creation_expression
    : 'new' anonymous_object_initializer
    ;

anonymous_object_initializer
    : '{' member_declarator_list? '}'
    | '{' member_declarator_list ',' '}'
    ;

member_declarator_list
    : member_declarator (',' member_declarator)*
    ;

member_declarator
    : simple_name
    | member_access
    | null_conditional_projection_initializer
    | base_access
    | identifier '=' expression
    ;

// Source: §12.8.17 The typeof operator
typeof_expression
    : 'typeof' '(' type ')'
    | 'typeof' '(' unbound_type_name ')'
    | 'typeof' '(' 'void' ')'
    ;

unbound_type_name
    : identifier generic_dimension_specifier?
    | identifier '::' identifier generic_dimension_specifier?
    | unbound_type_name '.' identifier generic_dimension_specifier?
    ;

generic_dimension_specifier
    : '<' comma* '>'
    ;

comma
    : ','
    ;


// Source: §12.8.18 The sizeof operator
sizeof_expression
    : 'sizeof' '(' unmanaged_type ')'
    ;

// Source: §12.8.19 The checked and unchecked operators
checked_expression
    : 'checked' '(' expression ')'
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
    : 'async'? anonymous_function_signature '=>' anonymous_function_body
    ;

anonymous_method_expression
    : 'async'? 'delegate' explicit_anonymous_function_signature? block
    ;

anonymous_function_signature
    : explicit_anonymous_function_signature
    | implicit_anonymous_function_signature
    ;

explicit_anonymous_function_signature
    : '(' explicit_anonymous_function_parameter_list? ')'
    ;

explicit_anonymous_function_parameter_list
    : explicit_anonymous_function_parameter
      (',' explicit_anonymous_function_parameter)*
    ;

explicit_anonymous_function_parameter
    : anonymous_function_parameter_modifier? type identifier
    ;

anonymous_function_parameter_modifier
    : 'ref'
    | 'out'
    | 'in'
    ;

implicit_anonymous_function_signature
    : '(' implicit_anonymous_function_parameter_list? ')'
    | implicit_anonymous_function_parameter
    ;

implicit_anonymous_function_parameter_list
    : implicit_anonymous_function_parameter
      (',' implicit_anonymous_function_parameter)*
    ;

implicit_anonymous_function_parameter
    : identifier
    ;

anonymous_function_body
    : null_conditional_invocation_expression
    | expression
    | 'ref' variable_reference
    | block
    ;

// Source: §12.20.1 General
query_expression
    : from_clause query_body
    ;

from_clause
    : 'from' type? identifier 'in' expression
    ;

query_body
    : query_body_clauses? select_or_group_clause query_continuation?
    ;

query_body_clauses
    : query_body_clause
    | query_body_clauses query_body_clause
    ;

query_body_clause
    : from_clause
    | let_clause
    | where_clause
    | join_clause
    | join_into_clause
    | orderby_clause
    ;

let_clause
    : 'let' identifier '=' expression
    ;

where_clause
    : 'where' boolean_expression
    ;

join_clause
    : 'join' type? identifier 'in' expression 'on' expression
      'equals' expression
    ;

join_into_clause
    : 'join' type? identifier 'in' expression 'on' expression
      'equals' expression 'into' identifier
    ;

orderby_clause
    : 'orderby' orderings
    ;

orderings
    : ordering (',' ordering)*
    ;

ordering
    : expression ordering_direction?
    ;

ordering_direction
    : 'ascending'
    | 'descending'
    ;

select_or_group_clause
    : select_clause
    | group_clause
    ;

select_clause
    : 'select' expression
    ;

group_clause
    : 'group' expression 'by' expression
    ;

query_continuation
    : 'into' identifier query_body
    ;

// Source: §12.21.1 General
assignment
    : unary_expression assignment_operator expression
    ;

assignment_operator
    : '=' 'ref'? | '+=' | '-=' | '*=' | '/=' | '%=' | '&=' | '|=' | '^=' | '<<='
    | right_shift_assignment
    ;

// Source: §12.22 Expression
expression
    : non_assignment_expression
    | assignment
    ;

non_assignment_expression
    : declaration_expression
    | conditional_expression
    | lambda_expression
    | query_expression
    ;

// Source: §12.23 Constant expressions
constant_expression
    : expression
    ;

// Source: §12.24 Boolean expressions
boolean_expression
    : expression
    ;

// Source: §13.1 General
statement
    : labeled_statement
    | declaration_statement
    | embedded_statement
    ;

embedded_statement
    : block
    | empty_statement
    | expression_statement
    | selection_statement
    | iteration_statement
    | jump_statement
    | try_statement
    | checked_statement
    | unchecked_statement
    | lock_statement
    | using_statement
    | yield_statement
    | unsafe_statement   // unsafe code support
    | fixed_statement    // unsafe code support
    ;

// Source: §13.3.1 General
block
    : '{' statement_list? '}'
    ;

// Source: §13.3.2 Statement lists
statement_list
    : statement+
    ;

// Source: §13.4 The empty statement
empty_statement
    : ';'
    ;

// Source: §13.5 Labeled statements
labeled_statement
    : identifier ':' statement
    ;

// Source: §13.6.1 General
declaration_statement
    : local_variable_declaration ';'
    | local_constant_declaration ';'
    | local_function_declaration
    ;

// Source: §13.6.2.1 General
local_variable_declaration
    : implicitly_typed_local_variable_declaration
    | explicitly_typed_local_variable_declaration
    | ref_local_variable_declaration
    ;

// Source: §13.6.2.2 Implicitly typed local variable declarations
implicitly_typed_local_variable_declaration
    : 'var' implicitly_typed_local_variable_declarator
    | ref_kind 'var' ref_local_variable_declarator
    ;

implicitly_typed_local_variable_declarator
    : identifier '=' expression
    ;

// Source: §13.6.2.3 Explicitly typed local variable declarations
explicitly_typed_local_variable_declaration
    : type explicitly_typed_local_variable_declarators
    ;

explicitly_typed_local_variable_declarators
    : explicitly_typed_local_variable_declarator
      (',' explicitly_typed_local_variable_declarator)*
    ;

explicitly_typed_local_variable_declarator
    : identifier ('=' local_variable_initializer)?
    ;

local_variable_initializer
    : expression
    | array_initializer
    ;

// Source: §13.6.2.4 Ref local variable declarations
ref_local_variable_declaration
    : ref_kind type ref_local_variable_declarators
    ;

ref_local_variable_declarators
    : ref_local_variable_declarator (',' ref_local_variable_declarator)*
    ;

ref_local_variable_declarator
    : identifier '=' 'ref' variable_reference
    ;

// Source: §13.6.3 Local constant declarations
local_constant_declaration
    : 'const' type constant_declarators
    ;

constant_declarators
    : constant_declarator (',' constant_declarator)*
    ;

constant_declarator
    : identifier '=' constant_expression
    ;

// Source: §13.6.4 Local function declarations
local_function_declaration
    : local_function_modifier* return_type local_function_header
      local_function_body
    | ref_local_function_modifier* ref_kind ref_return_type
      local_function_header ref_local_function_body
    ;

local_function_header
    : identifier '(' formal_parameter_list? ')'
    | identifier type_parameter_list '(' formal_parameter_list? ')'
      type_parameter_constraints_clause*
    ;

local_function_modifier
    : ref_local_function_modifier
    | 'async'
    ;

ref_local_function_modifier
    : 'static'
    | unsafe_modifier   // unsafe code support
    ;

local_function_body
    : block
    | '=>' null_conditional_invocation_expression ';'
    | '=>' expression ';'
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
    : 'case' pattern case_guard?  ':'
    | 'default' ':'
    ;

case_guard
    : 'when' expression
    ;

// Source: §13.9.1 General
iteration_statement
    : while_statement
    | do_statement
    | for_statement
    | foreach_statement
    ;

// Source: §13.9.2 The while statement
while_statement
    : 'while' '(' boolean_expression ')' embedded_statement
    ;

// Source: §13.9.3 The do statement
do_statement
    : 'do' embedded_statement 'while' '(' boolean_expression ')' ';'
    ;

// Source: §13.9.4 The for statement
for_statement
    : 'for' '(' for_initializer? ';' for_condition? ';' for_iterator? ')'
      embedded_statement
    ;

for_initializer
    : local_variable_declaration
    | statement_expression_list
    ;

for_condition
    : boolean_expression
    ;

for_iterator
    : statement_expression_list
    ;

statement_expression_list
    : statement_expression (',' statement_expression)*
    ;

// Source: §13.9.5 The foreach statement
foreach_statement
    : 'foreach' '(' ref_kind? local_variable_type identifier 'in' 
      expression ')' embedded_statement
    ;

// Source: §13.10.1 General
jump_statement
    : break_statement
    | continue_statement
    | goto_statement
    | return_statement
    | throw_statement
    ;

// Source: §13.10.2 The break statement
break_statement
    : 'break' ';'
    ;

// Source: §13.10.3 The continue statement
continue_statement
    : 'continue' ';'
    ;

// Source: §13.10.4 The goto statement
goto_statement
    : 'goto' identifier ';'
    | 'goto' 'case' constant_expression ';'
    | 'goto' 'default' ';'
    ;

// Source: §13.10.5 The return statement
return_statement
    : 'return' ';'
    | 'return' expression ';'
    | 'return' 'ref' variable_reference ';'
    ;

// Source: §13.10.6 The throw statement
throw_statement
    : 'throw' expression? ';'
    ;

// Source: §13.11 The try statement
try_statement
    : 'try' block catch_clauses
    | 'try' block catch_clauses? finally_clause
    ;

catch_clauses
    : specific_catch_clause+
    | specific_catch_clause* general_catch_clause
    ;

specific_catch_clause
    : 'catch' exception_specifier exception_filter? block
    | 'catch' exception_filter block
    ;

exception_specifier
    : '(' type identifier? ')'
    ;

exception_filter
    : 'when' '(' boolean_expression ')'
    ;

general_catch_clause
    : 'catch' block
    ;

finally_clause
    : 'finally' block
    ;

// Source: §13.12 The checked and unchecked statements
checked_statement
    : 'checked' block
    ;

unchecked_statement
    : 'unchecked' block
    ;

// Source: §13.13 The lock statement
lock_statement
    : 'lock' '(' expression ')' embedded_statement
    ;

// Source: §13.14 The using statement
using_statement
    : 'using' '(' resource_acquisition ')' embedded_statement
    ;

resource_acquisition
    : local_variable_declaration
    | expression
    ;

// Source: §13.15 The yield statement
yield_statement
    : 'yield' 'return' expression ';'
    | 'yield' 'break' ';'
    ;

// Source: §14.2 Compilation units
compilation_unit
    : extern_alias_directive* using_directive* global_attributes?
      namespace_member_declaration*
    ;

// Source: §14.3 Namespace declarations
namespace_declaration
    : 'namespace' qualified_identifier namespace_body ';'?
    ;

qualified_identifier
    : identifier ('.' identifier)*
    ;

namespace_body
    : '{' extern_alias_directive* using_directive*
      namespace_member_declaration* '}'
    ;

// Source: §14.4 Extern alias directives
extern_alias_directive
    : 'extern' 'alias' identifier ';'
    ;

// Source: §14.5.1 General
using_directive
    : using_alias_directive
    | using_namespace_directive
    | using_static_directive    
    ;

// Source: §14.5.2 Using alias directives
using_alias_directive
    : 'using' identifier '=' namespace_or_type_name ';'
    ;

// Source: §14.5.3 Using namespace directives
using_namespace_directive
    : 'using' namespace_name ';'
    ;

// Source: §14.5.4 Using static directives
using_static_directive
    : 'using' 'static' type_name ';'
    ;

// Source: §14.6 Namespace member declarations
namespace_member_declaration
    : namespace_declaration
    | type_declaration
    ;

// Source: §14.7 Type declarations
type_declaration
    : class_declaration
    | struct_declaration
    | interface_declaration
    | enum_declaration
    | delegate_declaration
    ;

// Source: §14.8.1 General
qualified_alias_member
    : identifier '::' identifier type_argument_list?
    ;

// Source: §15.2.1 General
class_declaration
    : attributes? class_modifier* 'partial'? 'class' identifier
        type_parameter_list? class_base? type_parameter_constraints_clause*
        class_body ';'?
    ;

// Source: §15.2.2.1 General
class_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'abstract'
    | 'sealed'
    | 'static'
    | unsafe_modifier   // unsafe code support
    ;

// Source: §15.2.3 Type parameters
type_parameter_list
    : '<' type_parameters '>'
  ;

type_parameters
    : attributes? type_parameter
    | type_parameters ',' attributes? type_parameter
    ;

// Source: §15.2.4.1 General
class_base
    : ':' class_type
    | ':' interface_type_list
    | ':' class_type ',' interface_type_list
    ;

interface_type_list
    : interface_type (',' interface_type)*
    ;

// Source: §15.2.5 Type parameter constraints
type_parameter_constraints_clauses
    : type_parameter_constraints_clause
    | type_parameter_constraints_clauses type_parameter_constraints_clause
    ;
    
type_parameter_constraints_clause
    : 'where' type_parameter ':' type_parameter_constraints
    ;

type_parameter_constraints
    : primary_constraint
    | secondary_constraints
    | constructor_constraint
    | primary_constraint ',' secondary_constraints
    | primary_constraint ',' constructor_constraint
    | secondary_constraints ',' constructor_constraint
    | primary_constraint ',' secondary_constraints ',' constructor_constraint
    ;

primary_constraint
    : class_type
    | 'class'
    | 'struct'
    | 'unmanaged'
    ;

secondary_constraints
    : interface_type
    | type_parameter
    | secondary_constraints ',' interface_type
    | secondary_constraints ',' type_parameter
    ;

constructor_constraint
    : 'new' '(' ')'
    ;

// Source: §15.2.6 Class body
class_body
    : '{' class_member_declaration* '}'
    ;

// Source: §15.3.1 General
class_member_declaration
    : constant_declaration
    | field_declaration
    | method_declaration
    | property_declaration
    | event_declaration
    | indexer_declaration
    | operator_declaration
    | constructor_declaration
    | finalizer_declaration
    | static_constructor_declaration
    | type_declaration
    ;

// Source: §15.4 Constants
constant_declaration
    : attributes? constant_modifier* 'const' type constant_declarators ';'
    ;

constant_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    ;

// Source: §15.5.1 General
field_declaration
    : attributes? field_modifier* type variable_declarators ';'
    ;

field_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'static'
    | 'readonly'
    | 'volatile'
    | unsafe_modifier   // unsafe code support
    ;

variable_declarators
    : variable_declarator (',' variable_declarator)*
    ;

variable_declarator
    : identifier ('=' variable_initializer)?
    ;

// Source: §15.6.1 General
method_declaration
    : attributes? method_modifiers return_type method_header method_body
    | attributes? ref_method_modifiers ref_kind ref_return_type method_header
      ref_method_body
    ;

method_modifiers
    : method_modifier* 'partial'?
    ;

ref_kind
    : 'ref'
    | 'ref' 'readonly'
    ;

ref_method_modifiers
    : ref_method_modifier*
    ;

method_header
    : member_name '(' formal_parameter_list? ')'
    | member_name type_parameter_list '(' formal_parameter_list? ')'
      type_parameter_constraints_clause*
    ;

method_modifier
    : ref_method_modifier
    | 'async'
    ;

ref_method_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'static'
    | 'virtual'
    | 'sealed'
    | 'override'
    | 'abstract'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;

return_type
    : ref_return_type
    | 'void'
    ;

ref_return_type
    : type
    ;

member_name
    : identifier
    | interface_type '.' identifier
    ;

method_body
    : block
    | '=>' null_conditional_invocation_expression ';'
    | '=>' expression ';'
    | ';'
    ;

ref_method_body
    : block
    | '=>' 'ref' variable_reference ';'
    | ';'
    ;

// Source: §15.6.2.1 General
formal_parameter_list
    : fixed_parameters
    | fixed_parameters ',' parameter_array
    | parameter_array
    ;

fixed_parameters
    : fixed_parameter (',' fixed_parameter)*
    ;

fixed_parameter
    : attributes? parameter_modifier? type identifier default_argument?
    ;

default_argument
    : '=' expression
    ;

parameter_modifier
    : parameter_mode_modifier
    | 'this'
    ;

parameter_mode_modifier
    : 'ref'
    | 'out'
    | 'in'
    ;

parameter_array
    : attributes? 'params' array_type identifier
    ;

// Source: §15.7.1 General
property_declaration
    : attributes? property_modifier* type member_name property_body
    | attributes? property_modifier* ref_kind type member_name ref_property_body
    ;    

property_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'static'
    | 'virtual'
    | 'sealed'
    | 'override'
    | 'abstract'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;
    
property_body
    : '{' accessor_declarations '}' property_initializer?
    | '=>' expression ';'
    ;

property_initializer
    : '=' variable_initializer ';'
    ;

ref_property_body
    : '{' ref_get_accessor_declaration '}'
    | '=>' 'ref' variable_reference ';'
    ;

// Source: §15.7.3 Accessors
accessor_declarations
    : get_accessor_declaration set_accessor_declaration?
    | set_accessor_declaration get_accessor_declaration?
    ;

get_accessor_declaration
    : attributes? accessor_modifier? 'get' accessor_body
    ;

set_accessor_declaration
    : attributes? accessor_modifier? 'set' accessor_body
    ;

accessor_modifier
    : 'protected'
    | 'internal'
    | 'private'
    | 'protected' 'internal'
    | 'internal' 'protected'
    | 'protected' 'private'
    | 'private' 'protected'
    ;

accessor_body
    : block
    | '=>' expression ';'
    | ';' 
    ;

ref_get_accessor_declaration
    : attributes? accessor_modifier? 'get' ref_accessor_body
    ;
    
ref_accessor_body
    : block
    | '=>' 'ref' variable_reference ';'
    | ';'
    ;

// Source: §15.8.1 General
event_declaration
    : attributes? event_modifier* 'event' type variable_declarators ';'
    | attributes? event_modifier* 'event' type member_name
        '{' event_accessor_declarations '}'
    ;

event_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'static'
    | 'virtual'
    | 'sealed'
    | 'override'
    | 'abstract'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;

event_accessor_declarations
    : add_accessor_declaration remove_accessor_declaration
    | remove_accessor_declaration add_accessor_declaration
    ;

add_accessor_declaration
    : attributes? 'add' block
    ;

remove_accessor_declaration
    : attributes? 'remove' block
    ;

// Source: §15.9.1 General
indexer_declaration
    : attributes? indexer_modifier* indexer_declarator indexer_body
    | attributes? indexer_modifier* ref_kind indexer_declarator ref_indexer_body
    ;

indexer_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'virtual'
    | 'sealed'
    | 'override'
    | 'abstract'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;

indexer_declarator
    : type 'this' '[' formal_parameter_list ']'
    | type interface_type '.' 'this' '[' formal_parameter_list ']'
    ;

indexer_body
    : '{' accessor_declarations '}' 
    | '=>' expression ';'
    ;  

ref_indexer_body
    : '{' ref_get_accessor_declaration '}'
    | '=>' 'ref' variable_reference ';'
    ;

// Source: §15.10.1 General
operator_declaration
    : attributes? operator_modifier+ operator_declarator operator_body
    ;

operator_modifier
    : 'public'
    | 'static'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;

operator_declarator
    : unary_operator_declarator
    | binary_operator_declarator
    | conversion_operator_declarator
    ;

unary_operator_declarator
    : type 'operator' overloadable_unary_operator '(' fixed_parameter ')'
    ;

overloadable_unary_operator
    : '+' | '-' | '!' | '~' | '++' | '--' | 'true' | 'false'
    ;

binary_operator_declarator
    : type 'operator' overloadable_binary_operator
        '(' fixed_parameter ',' fixed_parameter ')'
    ;

overloadable_binary_operator
    : '+'  | '-'  | '*'  | '/'  | '%'  | '&' | '|' | '^'  | '<<' 
    | right_shift | '==' | '!=' | '>' | '<' | '>=' | '<='
    ;

conversion_operator_declarator
    : 'implicit' 'operator' type '(' fixed_parameter ')'
    | 'explicit' 'operator' type '(' fixed_parameter ')'
    ;

operator_body
    : block
    | '=>' expression ';'
    | ';'
    ;

// Source: §15.11.1 General
constructor_declaration
    : attributes? constructor_modifier* constructor_declarator constructor_body
    ;

constructor_modifier
    : 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'extern'
    | unsafe_modifier   // unsafe code support
    ;

constructor_declarator
    : identifier '(' formal_parameter_list? ')' constructor_initializer?
    ;

constructor_initializer
    : ':' 'base' '(' argument_list? ')'
    | ':' 'this' '(' argument_list? ')'
    ;

constructor_body
    : block
    | '=>' expression ';'
    | ';'
    ;

// Source: §15.12 Static constructors
static_constructor_declaration
    : attributes? static_constructor_modifiers identifier '(' ')'
        static_constructor_body
    ;

static_constructor_modifiers
    : 'static'
    | 'static' 'extern' unsafe_modifier?
    | 'static' unsafe_modifier 'extern'?
    | 'extern' 'static' unsafe_modifier?
    | 'extern' unsafe_modifier 'static'
    | unsafe_modifier 'static' 'extern'?
    | unsafe_modifier 'extern' 'static'
    ;

static_constructor_body
    : block
    | '=>' expression ';'
    | ';'
    ;

// Source: §15.13 Finalizers
finalizer_declaration
    : attributes? '~' identifier '(' ')' finalizer_body
    | attributes? 'extern' unsafe_modifier? '~' identifier '(' ')'
      finalizer_body
    | attributes? unsafe_modifier 'extern'? '~' identifier '(' ')'
      finalizer_body
    ;

finalizer_body
    : block
    | '=>' expression ';'
    | ';'
    ;

// Source: §16.2.1 General
struct_declaration
    : attributes? struct_modifier* 'ref'? 'partial'? 'struct'
      identifier type_parameter_list? struct_interfaces?
      type_parameter_constraints_clause* struct_body ';'?
    ;

// Source: §16.2.2 Struct modifiers
struct_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | 'readonly'
    | unsafe_modifier   // unsafe code support
    ;

// Source: §16.2.5 Struct interfaces
struct_interfaces
    : ':' interface_type_list
    ;

// Source: §16.2.6 Struct body
struct_body
    : '{' struct_member_declaration* '}'
    ;

// Source: §16.3 Struct members
struct_member_declaration
    : constant_declaration
    | field_declaration
    | method_declaration
    | property_declaration
    | event_declaration
    | indexer_declaration
    | operator_declaration
    | constructor_declaration
    | static_constructor_declaration
    | type_declaration
    | fixed_size_buffer_declaration   // unsafe code support
    ;

// Source: §17.7 Array initializers
array_initializer
    : '{' variable_initializer_list? '}'
    | '{' variable_initializer_list ',' '}'
    ;

variable_initializer_list
    : variable_initializer (',' variable_initializer)*
    ;
    
variable_initializer
    : expression
    | array_initializer
    ;

// Source: §18.2.1 General
interface_declaration
    : attributes? interface_modifier* 'partial'? 'interface'
      identifier variant_type_parameter_list? interface_base?
      type_parameter_constraints_clause* interface_body ';'?
    ;

// Source: §18.2.2 Interface modifiers
interface_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | unsafe_modifier   // unsafe code support
    ;

// Source: §18.2.3.1 General
variant_type_parameter_list
    : '<' variant_type_parameters '>'
    ;

// Source: §18.2.3.1 General
variant_type_parameters
    : attributes? variance_annotation? type_parameter
    | variant_type_parameters ',' attributes? variance_annotation?
      type_parameter
    ;

// Source: §18.2.3.1 General
variance_annotation
    : 'in'
    | 'out'
    ;

// Source: §18.2.4 Base interfaces
interface_base
    : ':' interface_type_list
    ;

// Source: §18.3 Interface body
interface_body
    : '{' interface_member_declaration* '}'
    ;

// Source: §18.4.1 General
interface_member_declaration
    : interface_method_declaration
    | interface_property_declaration
    | interface_event_declaration
    | interface_indexer_declaration
    ;

// Source: §18.4.2 Interface methods
interface_method_declaration
    : attributes? 'new'? return_type interface_method_header
    | attributes? 'new'? ref_kind ref_return_type interface_method_header
    ;

interface_method_header
    : identifier '(' formal_parameter_list? ')' ';'
    | identifier type_parameter_list '(' formal_parameter_list? ')'
      type_parameter_constraints_clause* ';'
    ;

// Source: §18.4.3 Interface properties
interface_property_declaration
    : attributes? 'new'? type identifier '{' interface_accessors '}'
    | attributes? 'new'? ref_kind type identifier '{' ref_interface_accessor '}'
    ;

interface_accessors
    : attributes? 'get' ';'
    | attributes? 'set' ';'
    | attributes? 'get' ';' attributes? 'set' ';'
    | attributes? 'set' ';' attributes? 'get' ';'
    ;

ref_interface_accessor
    : attributes? 'get' ';'
    ;

// Source: §18.4.4 Interface events
interface_event_declaration
    : attributes? 'new'? 'event' type identifier ';'
    ;

// Source: §18.4.5 Interface indexers
interface_indexer_declaration
    : attributes? 'new'? type 'this' '[' formal_parameter_list ']'
      '{' interface_accessors '}'
    | attributes? 'new'? ref_kind type 'this' '[' formal_parameter_list ']'
      '{' ref_interface_accessor '}'
    ;

// Source: §19.2 Enum declarations
enum_declaration
    : attributes? enum_modifier* 'enum' identifier enum_base? enum_body ';'?
    ;

enum_base
    : ':' integral_type
    | ':' integral_type_name
    ;

integral_type_name
    : type_name // Shall resolve to an integral type other than char
    ;

enum_body
    : '{' enum_member_declarations? '}'
    | '{' enum_member_declarations ',' '}'
    ;

// Source: §19.3 Enum modifiers
enum_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    ;

// Source: §19.4 Enum members
enum_member_declarations
    : enum_member_declaration (',' enum_member_declaration)*
    ;

// Source: §19.4 Enum members
enum_member_declaration
    : attributes? identifier ('=' constant_expression)?
    ;

// Source: §20.2 Delegate declarations
delegate_declaration
    : attributes? delegate_modifier* 'delegate' return_type delegate_header
    | attributes? delegate_modifier* 'delegate' ref_kind ref_return_type
      delegate_header
    ;

delegate_header
    : identifier '(' formal_parameter_list? ')' ';'
    | identifier variant_type_parameter_list '(' formal_parameter_list? ')'
      type_parameter_constraints_clause* ';'
    ;
    
delegate_modifier
    : 'new'
    | 'public'
    | 'protected'
    | 'internal'
    | 'private'
    | unsafe_modifier   // unsafe code support
    ;

// Source: §22.3 Attribute specification
global_attributes
    : global_attribute_section+
    ;

global_attribute_section
    : '[' global_attribute_target_specifier attribute_list ']'
    | '[' global_attribute_target_specifier attribute_list ',' ']'
    ;

global_attribute_target_specifier
    : global_attribute_target ':'
    ;

global_attribute_target
    : identifier
    ;

attributes
    : attribute_section+
    ;

attribute_section
    : '[' attribute_target_specifier? attribute_list ']'
    | '[' attribute_target_specifier? attribute_list ',' ']'
    ;

attribute_target_specifier
    : attribute_target ':'
    ;

attribute_target
    : identifier
    | keyword
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

// Source: §6.4.3 Identifiers
identifier
    : Simple_Identifier
    | contextual_keyword
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

// ##################################################################################################
// lexer rules
// ##################################################################################################

// Source: §6.3.1 General
DEFAULT  : 'default' ;
NULL     : 'null' ;
TRUE     : 'true' ;
FALSE    : 'false' ;
ASTERISK : '*' ;
SLASH    : '/' ;

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
    : UnicodeZs  // any character with Unicode class Zs
    | '\u0009'  // horizontal tab
    | '\u000B'  // vertical tab
    | '\u000C'  // form feed
    ;



// Source: §6.4.2 Unicode character escape sequences
fragment Unicode_Escape_Sequence
    : '\\u' Hex_Digit Hex_Digit Hex_Digit Hex_Digit
    | '\\U' Hex_Digit Hex_Digit Hex_Digit Hex_Digit
            Hex_Digit Hex_Digit Hex_Digit Hex_Digit
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

fragment Identifier_Start_Character
    : Letter_Character
    | Underscore_Character
    ;

fragment Underscore_Character
    : '_'               // underscore
    | '\\u005' [fF]     // Unicode_Escape_Sequence for underscore
    | '\\U0000005' [fF] // Unicode_Escape_Sequence for underscore
    ;

fragment Identifier_Part_Character
    : Letter_Character
    | Decimal_Digit_Character
    | Connecting_Character
    | Combining_Character
    | Formatting_Character
    ;

fragment Letter_Character
    // Category Letter, all subcategories; category Number, subcategory letter.
    : UnicodeL 
    | UnicodeNl
    // Only escapes for categories L & Nl allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Combining_Character
    // Category Mark, subcategories non-spacing and spacing combining.
    : UnicodeMn 
    | UnicodeMc
    // Only escapes for categories Mn & Mc allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Decimal_Digit_Character
    // Category Number, subcategory decimal digit.
    : UnicodeNd
    // Only escapes for category Nd allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Connecting_Character
    // Category Punctuation, subcategory connector.
    : UnicodePc
    // Only escapes for category Pc allowed. See note below.
    | Unicode_Escape_Sequence
    ;

fragment Formatting_Character
    // Category Other, subcategory format.
    : UnicodeCf
    // Only escapes for category Cf allowed, see note below.
    | Unicode_Escape_Sequence
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
    : ( UnicodeZs  // any character with Unicode class Zs
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

// Source: §6.5.6 Diagnostic directives
fragment PP_Diagnostic
    : 'error' PP_Message?
    | 'warning' PP_Message?
    ;

fragment PP_Message
    : PP_Whitespace Input_Character*
    ;

// Source: §6.5.7 Region directives
fragment PP_Region
    : PP_Start_Region
    | PP_End_Region
    ;

fragment PP_Start_Region
    : 'region' PP_Message?
    ;

fragment PP_End_Region
    : 'endregion' PP_Message?
    ;

// Source: §6.5.8 Line directives
fragment PP_Line
    : 'line' PP_Whitespace PP_Line_Indicator
    ;

fragment PP_Line_Indicator
    : Decimal_Digit+ PP_Whitespace PP_Compilation_Unit_Name
    | Decimal_Digit+
    | DEFAULT
    | 'hidden'
    ;
    
fragment PP_Compilation_Unit_Name
    : '"' PP_Compilation_Unit_Name_Character+ '"'
    ;
    
fragment PP_Compilation_Unit_Name_Character
    // Any Input_Character except "
    : ~('\u000D' | '\u000A'   | '\u0085' | '\u2028' | '\u2029' | '#')
    ;

// Source: §6.5.9 Pragma directives
fragment PP_Pragma
    : 'pragma' PP_Pragma_Text?
    ;

fragment PP_Pragma_Text
    : PP_Whitespace Input_Character*
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

fragment UnicodePc
    : '\u005F'
    | '\u203F' .. '\u2040'
    | '\u2054'
    | '\uFE33' .. '\uFE34'
    | '\uFE4D' .. '\uFE4F'
    | '\uFF3F'
    ;

fragment UnicodeNd
    : '\u0030' .. '\u0039'
    | '\u0660' .. '\u0669'
    | '\u06F0' .. '\u06F9'
    | '\u07C0' .. '\u07C9'
    | '\u0966' .. '\u096F'
    | '\u09E6' .. '\u09EF'
    | '\u0A66' .. '\u0A6F'
    | '\u0AE6' .. '\u0AEF'
    | '\u0B66' .. '\u0B6F'
    | '\u0BE6' .. '\u0BEF'
    | '\u0C66' .. '\u0C6F'
    | '\u0CE6' .. '\u0CEF'
    | '\u0D66' .. '\u0D6F'
    | '\u0DE6' .. '\u0DEF'
    | '\u0E50' .. '\u0E59'
    | '\u0ED0' .. '\u0ED9'
    | '\u0F20' .. '\u0F29'
    | '\u1040' .. '\u1049'
    | '\u1090' .. '\u1099'
    | '\u17E0' .. '\u17E9'
    | '\u1810' .. '\u1819'
    | '\u1946' .. '\u194F'
    | '\u19D0' .. '\u19D9'
    | '\u1A80' .. '\u1A89'
    | '\u1A90' .. '\u1A99'
    | '\u1B50' .. '\u1B59'
    | '\u1BB0' .. '\u1BB9'
    | '\u1C40' .. '\u1C49'
    | '\u1C50' .. '\u1C59'
    | '\uA620' .. '\uA629'
    | '\uA8D0' .. '\uA8D9'
    | '\uA900' .. '\uA909'
    | '\uA9D0' .. '\uA9D9'
    | '\uA9F0' .. '\uA9F9'
    | '\uAA50' .. '\uAA59'
    | '\uABF0' .. '\uABF9'
    | '\uFF10' .. '\uFF19'
    ;

fragment UnicodeMc
    : '\u0903'
    | '\u093B'
    | '\u093E' .. '\u0940'
    | '\u0949' .. '\u094C'
    | '\u094E' .. '\u094F'
    | '\u0982' .. '\u0983'
    | '\u09BE' .. '\u09C0'
    | '\u09C7' .. '\u09C8'
    | '\u09CB' .. '\u09CC'
    | '\u09D7'
    | '\u0A03'
    | '\u0A3E' .. '\u0A40'
    | '\u0A83'
    | '\u0ABE' .. '\u0AC0'
    | '\u0AC9'
    | '\u0ACB' .. '\u0ACC'
    | '\u0B02' .. '\u0B03'
    | '\u0B3E'
    | '\u0B40'
    | '\u0B47' .. '\u0B48'
    | '\u0B4B' .. '\u0B4C'
    | '\u0B57'
    | '\u0BBE' .. '\u0BBF'
    | '\u0BC1' .. '\u0BC2'
    | '\u0BC6' .. '\u0BC8'
    | '\u0BCA' .. '\u0BCC'
    | '\u0BD7'
    | '\u0C01' .. '\u0C03'
    | '\u0C41' .. '\u0C44'
    | '\u0C82' .. '\u0C83'
    | '\u0CBE'
    | '\u0CC0' .. '\u0CC4'
    | '\u0CC7' .. '\u0CC8'
    | '\u0CCA' .. '\u0CCB'
    | '\u0CD5' .. '\u0CD6'
    | '\u0D02' .. '\u0D03'
    | '\u0D3E' .. '\u0D40'
    | '\u0D46' .. '\u0D48'
    | '\u0D4A' .. '\u0D4C'
    | '\u0D57'
    | '\u0D82' .. '\u0D83'
    | '\u0DCF' .. '\u0DD1'
    | '\u0DD8' .. '\u0DDF'
    | '\u0DF2' .. '\u0DF3'
    | '\u0F3E' .. '\u0F3F'
    | '\u0F7F'
    | '\u102B' .. '\u102C'
    | '\u1031'
    | '\u1038'
    | '\u103B' .. '\u103C'
    | '\u1056' .. '\u1057'
    | '\u1062' .. '\u1064'
    | '\u1067' .. '\u106D'
    | '\u1083' .. '\u1084'
    | '\u1087' .. '\u108C'
    | '\u108F'
    | '\u109A' .. '\u109C'
    | '\u17B6'
    | '\u17BE' .. '\u17C5'
    | '\u17C7' .. '\u17C8'
    | '\u1923' .. '\u1926'
    | '\u1929' .. '\u192B'
    | '\u1930' .. '\u1931'
    | '\u1933' .. '\u1938'
    | '\u1A19' .. '\u1A1A'
    | '\u1A55'
    | '\u1A57'
    | '\u1A61'
    | '\u1A63' .. '\u1A64'
    | '\u1A6D' .. '\u1A72'
    | '\u1B04'
    | '\u1B35'
    | '\u1B3B'
    | '\u1B3D' .. '\u1B41'
    | '\u1B43' .. '\u1B44'
    | '\u1B82'
    | '\u1BA1'
    | '\u1BA6' .. '\u1BA7'
    | '\u1BAA'
    | '\u1BE7'
    | '\u1BEA' .. '\u1BEC'
    | '\u1BEE'
    | '\u1BF2' .. '\u1BF3'
    | '\u1C24' .. '\u1C2B'
    | '\u1C34' .. '\u1C35'
    | '\u1CE1'
    | '\u1CF7'
    | '\u302E' .. '\u302F'
    | '\uA823' .. '\uA824'
    | '\uA827'
    | '\uA880' .. '\uA881'
    | '\uA8B4' .. '\uA8C3'
    | '\uA952' .. '\uA953'
    | '\uA983'
    | '\uA9B4' .. '\uA9B5'
    | '\uA9BA' .. '\uA9BB'
    | '\uA9BE' .. '\uA9C0'
    | '\uAA2F' .. '\uAA30'
    | '\uAA33' .. '\uAA34'
    | '\uAA4D'
    | '\uAA7B'
    | '\uAA7D'
    | '\uAAEB'
    | '\uAAEE' .. '\uAAEF'
    | '\uAAF5'
    | '\uABE3' .. '\uABE4'
    | '\uABE6' .. '\uABE7'
    | '\uABE9' .. '\uABEA'
    | '\uABEC'
    ;

fragment UnicodeMn
    : '\u0300' .. '\u036F'
    | '\u0483' .. '\u0487'
    | '\u0591' .. '\u05BD'
    | '\u05BF'
    | '\u05C1' .. '\u05C2'
    | '\u05C4' .. '\u05C5'
    | '\u05C7'
    | '\u0610' .. '\u061A'
    | '\u064B' .. '\u065F'
    | '\u0670'
    | '\u06D6' .. '\u06DC'
    | '\u06DF' .. '\u06E4'
    | '\u06E7' .. '\u06E8'
    | '\u06EA' .. '\u06ED'
    | '\u0711'
    | '\u0730' .. '\u074A'
    | '\u07A6' .. '\u07B0'
    | '\u07EB' .. '\u07F3'
    | '\u07FD'
    | '\u0816' .. '\u0819'
    | '\u081B' .. '\u0823'
    | '\u0825' .. '\u0827'
    | '\u0829' .. '\u082D'
    | '\u0859' .. '\u085B'
    | '\u08D3' .. '\u08E1'
    | '\u08E3' .. '\u0902'
    | '\u093A'
    | '\u093C'
    | '\u0941' .. '\u0948'
    | '\u094D'
    | '\u0951' .. '\u0957'
    | '\u0962' .. '\u0963'
    | '\u0981'
    | '\u09BC'
    | '\u09C1' .. '\u09C4'
    | '\u09CD'
    | '\u09E2' .. '\u09E3'
    | '\u09FE'
    | '\u0A01' .. '\u0A02'
    | '\u0A3C'
    | '\u0A41' .. '\u0A42'
    | '\u0A47' .. '\u0A48'
    | '\u0A4B' .. '\u0A4D'
    | '\u0A51'
    | '\u0A70' .. '\u0A71'
    | '\u0A75'
    | '\u0A81' .. '\u0A82'
    | '\u0ABC'
    | '\u0AC1' .. '\u0AC5'
    | '\u0AC7' .. '\u0AC8'
    | '\u0ACD'
    | '\u0AE2' .. '\u0AE3'
    | '\u0AFA' .. '\u0AFF'
    | '\u0B01'
    | '\u0B3C'
    | '\u0B3F'
    | '\u0B41' .. '\u0B44'
    | '\u0B4D'
    | '\u0B55' .. '\u0B56'
    | '\u0B62' .. '\u0B63'
    | '\u0B82'
    | '\u0BC0'
    | '\u0BCD'
    | '\u0C00'
    | '\u0C04'
    | '\u0C3E' .. '\u0C40'
    | '\u0C46' .. '\u0C48'
    | '\u0C4A' .. '\u0C4D'
    | '\u0C55' .. '\u0C56'
    | '\u0C62' .. '\u0C63'
    | '\u0C81'
    | '\u0CBC'
    | '\u0CBF'
    | '\u0CC6'
    | '\u0CCC' .. '\u0CCD'
    | '\u0CE2' .. '\u0CE3'
    | '\u0D00' .. '\u0D01'
    | '\u0D3B' .. '\u0D3C'
    | '\u0D41' .. '\u0D44'
    | '\u0D4D'
    | '\u0D62' .. '\u0D63'
    | '\u0D81'
    | '\u0DCA'
    | '\u0DD2' .. '\u0DD4'
    | '\u0DD6'
    | '\u0E31'
    | '\u0E34' .. '\u0E3A'
    | '\u0E47' .. '\u0E4E'
    | '\u0EB1'
    | '\u0EB4' .. '\u0EBC'
    | '\u0EC8' .. '\u0ECD'
    | '\u0F18' .. '\u0F19'
    | '\u0F35'
    | '\u0F37'
    | '\u0F39'
    | '\u0F71' .. '\u0F7E'
    | '\u0F80' .. '\u0F84'
    | '\u0F86' .. '\u0F87'
    | '\u0F8D' .. '\u0F97'
    | '\u0F99' .. '\u0FBC'
    | '\u0FC6'
    | '\u102D' .. '\u1030'
    | '\u1032' .. '\u1037'
    | '\u1039' .. '\u103A'
    | '\u103D' .. '\u103E'
    | '\u1058' .. '\u1059'
    | '\u105E' .. '\u1060'
    | '\u1071' .. '\u1074'
    | '\u1082'
    | '\u1085' .. '\u1086'
    | '\u108D'
    | '\u109D'
    | '\u135D' .. '\u135F'
    | '\u1712' .. '\u1714'
    | '\u1732' .. '\u1734'
    | '\u1752' .. '\u1753'
    | '\u1772' .. '\u1773'
    | '\u17B4' .. '\u17B5'
    | '\u17B7' .. '\u17BD'
    | '\u17C6'
    | '\u17C9' .. '\u17D3'
    | '\u17DD'
    | '\u180B' .. '\u180D'
    | '\u1885' .. '\u1886'
    | '\u18A9'
    | '\u1920' .. '\u1922'
    | '\u1927' .. '\u1928'
    | '\u1932'
    | '\u1939' .. '\u193B'
    | '\u1A17' .. '\u1A18'
    | '\u1A1B'
    | '\u1A56'
    | '\u1A58' .. '\u1A5E'
    | '\u1A60'
    | '\u1A62'
    | '\u1A65' .. '\u1A6C'
    | '\u1A73' .. '\u1A7C'
    | '\u1A7F'
    | '\u1AB0' .. '\u1ABD'
    | '\u1ABF' .. '\u1AC0'
    | '\u1B00' .. '\u1B03'
    | '\u1B34'
    | '\u1B36' .. '\u1B3A'
    | '\u1B3C'
    | '\u1B42'
    | '\u1B6B' .. '\u1B73'
    | '\u1B80' .. '\u1B81'
    | '\u1BA2' .. '\u1BA5'
    | '\u1BA8' .. '\u1BA9'
    | '\u1BAB' .. '\u1BAD'
    | '\u1BE6'
    | '\u1BE8' .. '\u1BE9'
    | '\u1BED'
    | '\u1BEF' .. '\u1BF1'
    | '\u1C2C' .. '\u1C33'
    | '\u1C36' .. '\u1C37'
    | '\u1CD0' .. '\u1CD2'
    | '\u1CD4' .. '\u1CE0'
    | '\u1CE2' .. '\u1CE8'
    | '\u1CED'
    | '\u1CF4'
    | '\u1CF8' .. '\u1CF9'
    | '\u1DC0' .. '\u1DF9'
    | '\u1DFB' .. '\u1DFF'
    | '\u20D0' .. '\u20DC'
    | '\u20E1'
    | '\u20E5' .. '\u20F0'
    | '\u2CEF' .. '\u2CF1'
    | '\u2D7F'
    | '\u2DE0' .. '\u2DFF'
    | '\u302A' .. '\u302D'
    | '\u3099' .. '\u309A'
    | '\uA66F'
    | '\uA674' .. '\uA67D'
    | '\uA69E' .. '\uA69F'
    | '\uA6F0' .. '\uA6F1'
    | '\uA802'
    | '\uA806'
    | '\uA80B'
    | '\uA825' .. '\uA826'
    | '\uA82C'
    | '\uA8C4' .. '\uA8C5'
    | '\uA8E0' .. '\uA8F1'
    | '\uA8FF'
    | '\uA926' .. '\uA92D'
    | '\uA947' .. '\uA951'
    | '\uA980' .. '\uA982'
    | '\uA9B3'
    | '\uA9B6' .. '\uA9B9'
    | '\uA9BC' .. '\uA9BD'
    | '\uA9E5'
    | '\uAA29' .. '\uAA2E'
    | '\uAA31' .. '\uAA32'
    | '\uAA35' .. '\uAA36'
    | '\uAA43'
    | '\uAA4C'
    | '\uAA7C'
    | '\uAAB0'
    | '\uAAB2' .. '\uAAB4'
    | '\uAAB7' .. '\uAAB8'
    | '\uAABE' .. '\uAABF'
    | '\uAAC1'
    | '\uAAEC' .. '\uAAED'
    | '\uAAF6'
    | '\uABE5'
    | '\uABE8'
    | '\uABED'
    | '\uFB1E'
    | '\uFE00' .. '\uFE0F'
    | '\uFE20' .. '\uFE2F'
    ;

fragment UnicodeNl
    : '\u16EE' .. '\u16F0'
    | '\u2160' .. '\u2182'
    | '\u2185' .. '\u2188'
    | '\u3007'
    | '\u3021' .. '\u3029'
    | '\u3038' .. '\u303A'
    | '\uA6E6' .. '\uA6EF'
    ;

fragment UnicodeZs
    : '\u0020'
    | '\u00A0'
    | '\u1680'
    | '\u2000' .. '\u200A'
    | '\u202F'
    | '\u205F'
    | '\u3000'
    ;

fragment UnicodeCf
    : '\u00AD'
    | '\u0600' .. '\u0605'
    | '\u061C'
    | '\u06DD'
    | '\u070F'
    | '\u08E2'
    | '\u180E'
    | '\u200B' .. '\u200F'
    | '\u202A' .. '\u202E'
    | '\u2060' .. '\u2064'
    | '\u2066' .. '\u206F'
    | '\uFEFF'
    | '\uFFF9' .. '\uFFFB'
    ;

fragment UnicodeL
    : UnicodeLu
    | UnicodeLl
    | UnicodeLt
    | UnicodeLm
    | UnicodeLo
    ;

fragment UnicodeLt
    : '\u01C5'
    | '\u01C8'
    | '\u01CB'
    | '\u01F2'
    | '\u1F88' .. '\u1F8F'
    | '\u1F98' .. '\u1F9F'
    | '\u1FA8' .. '\u1FAF'
    | '\u1FBC'
    | '\u1FCC'
    | '\u1FFC'
    ;

fragment UnicodeLm
    : '\u02B0' .. '\u02C1'
    | '\u02C6' .. '\u02D1'
    | '\u02E0' .. '\u02E4'
    | '\u02EC'
    | '\u02EE'
    | '\u0374'
    | '\u037A'
    | '\u0559'
    | '\u0640'
    | '\u06E5' .. '\u06E6'
    | '\u07F4' .. '\u07F5'
    | '\u07FA'
    | '\u081A'
    | '\u0824'
    | '\u0828'
    | '\u0971'
    | '\u0E46'
    | '\u0EC6'
    | '\u10FC'
    | '\u17D7'
    | '\u1843'
    | '\u1AA7'
    | '\u1C78' .. '\u1C7D'
    | '\u1D2C' .. '\u1D6A'
    | '\u1D78'
    | '\u1D9B' .. '\u1DBF'
    | '\u2071'
    | '\u207F'
    | '\u2090' .. '\u209C'
    | '\u2C7C' .. '\u2C7D'
    | '\u2D6F'
    | '\u2E2F'
    | '\u3005'
    | '\u3031' .. '\u3035'
    | '\u303B'
    | '\u309D' .. '\u309E'
    | '\u30FC' .. '\u30FE'
    | '\uA015'
    | '\uA4F8' .. '\uA4FD'
    | '\uA60C'
    | '\uA67F'
    | '\uA69C' .. '\uA69D'
    | '\uA717' .. '\uA71F'
    | '\uA770'
    | '\uA788'
    | '\uA7F8' .. '\uA7F9'
    | '\uA9CF'
    | '\uA9E6'
    | '\uAA70'
    | '\uAADD'
    | '\uAAF3' .. '\uAAF4'
    | '\uAB5C' .. '\uAB5F'
    | '\uAB69'
    | '\uFF70'
    | '\uFF9E' .. '\uFF9F'
    ;

fragment UnicodeLu
    : '\u0041' .. '\u005A'
    | '\u00C0' .. '\u00D6'
    | '\u00D8' .. '\u00DE'
    | '\u0100'
    | '\u0102'
    | '\u0104'
    | '\u0106'
    | '\u0108'
    | '\u010A'
    | '\u010C'
    | '\u010E'
    | '\u0110'
    | '\u0112'
    | '\u0114'
    | '\u0116'
    | '\u0118'
    | '\u011A'
    | '\u011C'
    | '\u011E'
    | '\u0120'
    | '\u0122'
    | '\u0124'
    | '\u0126'
    | '\u0128'
    | '\u012A'
    | '\u012C'
    | '\u012E'
    | '\u0130'
    | '\u0132'
    | '\u0134'
    | '\u0136'
    | '\u0139'
    | '\u013B'
    | '\u013D'
    | '\u013F'
    | '\u0141'
    | '\u0143'
    | '\u0145'
    | '\u0147'
    | '\u014A'
    | '\u014C'
    | '\u014E'
    | '\u0150'
    | '\u0152'
    | '\u0154'
    | '\u0156'
    | '\u0158'
    | '\u015A'
    | '\u015C'
    | '\u015E'
    | '\u0160'
    | '\u0162'
    | '\u0164'
    | '\u0166'
    | '\u0168'
    | '\u016A'
    | '\u016C'
    | '\u016E'
    | '\u0170'
    | '\u0172'
    | '\u0174'
    | '\u0176'
    | '\u0178' .. '\u0179'
    | '\u017B'
    | '\u017D'
    | '\u0181' .. '\u0182'
    | '\u0184'
    | '\u0186' .. '\u0187'
    | '\u0189' .. '\u018B'
    | '\u018E' .. '\u0191'
    | '\u0193' .. '\u0194'
    | '\u0196' .. '\u0198'
    | '\u019C' .. '\u019D'
    | '\u019F' .. '\u01A0'
    | '\u01A2'
    | '\u01A4'
    | '\u01A6' .. '\u01A7'
    | '\u01A9'
    | '\u01AC'
    | '\u01AE' .. '\u01AF'
    | '\u01B1' .. '\u01B3'
    | '\u01B5'
    | '\u01B7' .. '\u01B8'
    | '\u01BC'
    | '\u01C4'
    | '\u01C7'
    | '\u01CA'
    | '\u01CD'
    | '\u01CF'
    | '\u01D1'
    | '\u01D3'
    | '\u01D5'
    | '\u01D7'
    | '\u01D9'
    | '\u01DB'
    | '\u01DE'
    | '\u01E0'
    | '\u01E2'
    | '\u01E4'
    | '\u01E6'
    | '\u01E8'
    | '\u01EA'
    | '\u01EC'
    | '\u01EE'
    | '\u01F1'
    | '\u01F4'
    | '\u01F6' .. '\u01F8'
    | '\u01FA'
    | '\u01FC'
    | '\u01FE'
    | '\u0200'
    | '\u0202'
    | '\u0204'
    | '\u0206'
    | '\u0208'
    | '\u020A'
    | '\u020C'
    | '\u020E'
    | '\u0210'
    | '\u0212'
    | '\u0214'
    | '\u0216'
    | '\u0218'
    | '\u021A'
    | '\u021C'
    | '\u021E'
    | '\u0220'
    | '\u0222'
    | '\u0224'
    | '\u0226'
    | '\u0228'
    | '\u022A'
    | '\u022C'
    | '\u022E'
    | '\u0230'
    | '\u0232'
    | '\u023A' .. '\u023B'
    | '\u023D' .. '\u023E'
    | '\u0241'
    | '\u0243' .. '\u0246'
    | '\u0248'
    | '\u024A'
    | '\u024C'
    | '\u024E'
    | '\u0370'
    | '\u0372'
    | '\u0376'
    | '\u037F'
    | '\u0386'
    | '\u0388' .. '\u038A'
    | '\u038C'
    | '\u038E' .. '\u038F'
    | '\u0391' .. '\u03A1'
    | '\u03A3' .. '\u03AB'
    | '\u03CF'
    | '\u03D2' .. '\u03D4'
    | '\u03D8'
    | '\u03DA'
    | '\u03DC'
    | '\u03DE'
    | '\u03E0'
    | '\u03E2'
    | '\u03E4'
    | '\u03E6'
    | '\u03E8'
    | '\u03EA'
    | '\u03EC'
    | '\u03EE'
    | '\u03F4'
    | '\u03F7'
    | '\u03F9' .. '\u03FA'
    | '\u03FD' .. '\u042F'
    | '\u0460'
    | '\u0462'
    | '\u0464'
    | '\u0466'
    | '\u0468'
    | '\u046A'
    | '\u046C'
    | '\u046E'
    | '\u0470'
    | '\u0472'
    | '\u0474'
    | '\u0476'
    | '\u0478'
    | '\u047A'
    | '\u047C'
    | '\u047E'
    | '\u0480'
    | '\u048A'
    | '\u048C'
    | '\u048E'
    | '\u0490'
    | '\u0492'
    | '\u0494'
    | '\u0496'
    | '\u0498'
    | '\u049A'
    | '\u049C'
    | '\u049E'
    | '\u04A0'
    | '\u04A2'
    | '\u04A4'
    | '\u04A6'
    | '\u04A8'
    | '\u04AA'
    | '\u04AC'
    | '\u04AE'
    | '\u04B0'
    | '\u04B2'
    | '\u04B4'
    | '\u04B6'
    | '\u04B8'
    | '\u04BA'
    | '\u04BC'
    | '\u04BE'
    | '\u04C0' .. '\u04C1'
    | '\u04C3'
    | '\u04C5'
    | '\u04C7'
    | '\u04C9'
    | '\u04CB'
    | '\u04CD'
    | '\u04D0'
    | '\u04D2'
    | '\u04D4'
    | '\u04D6'
    | '\u04D8'
    | '\u04DA'
    | '\u04DC'
    | '\u04DE'
    | '\u04E0'
    | '\u04E2'
    | '\u04E4'
    | '\u04E6'
    | '\u04E8'
    | '\u04EA'
    | '\u04EC'
    | '\u04EE'
    | '\u04F0'
    | '\u04F2'
    | '\u04F4'
    | '\u04F6'
    | '\u04F8'
    | '\u04FA'
    | '\u04FC'
    | '\u04FE'
    | '\u0500'
    | '\u0502'
    | '\u0504'
    | '\u0506'
    | '\u0508'
    | '\u050A'
    | '\u050C'
    | '\u050E'
    | '\u0510'
    | '\u0512'
    | '\u0514'
    | '\u0516'
    | '\u0518'
    | '\u051A'
    | '\u051C'
    | '\u051E'
    | '\u0520'
    | '\u0522'
    | '\u0524'
    | '\u0526'
    | '\u0528'
    | '\u052A'
    | '\u052C'
    | '\u052E'
    | '\u0531' .. '\u0556'
    | '\u10A0' .. '\u10C5'
    | '\u10C7'
    | '\u10CD'
    | '\u13A0' .. '\u13F5'
    | '\u1C90' .. '\u1CBA'
    | '\u1CBD' .. '\u1CBF'
    | '\u1E00'
    | '\u1E02'
    | '\u1E04'
    | '\u1E06'
    | '\u1E08'
    | '\u1E0A'
    | '\u1E0C'
    | '\u1E0E'
    | '\u1E10'
    | '\u1E12'
    | '\u1E14'
    | '\u1E16'
    | '\u1E18'
    | '\u1E1A'
    | '\u1E1C'
    | '\u1E1E'
    | '\u1E20'
    | '\u1E22'
    | '\u1E24'
    | '\u1E26'
    | '\u1E28'
    | '\u1E2A'
    | '\u1E2C'
    | '\u1E2E'
    | '\u1E30'
    | '\u1E32'
    | '\u1E34'
    | '\u1E36'
    | '\u1E38'
    | '\u1E3A'
    | '\u1E3C'
    | '\u1E3E'
    | '\u1E40'
    | '\u1E42'
    | '\u1E44'
    | '\u1E46'
    | '\u1E48'
    | '\u1E4A'
    | '\u1E4C'
    | '\u1E4E'
    | '\u1E50'
    | '\u1E52'
    | '\u1E54'
    | '\u1E56'
    | '\u1E58'
    | '\u1E5A'
    | '\u1E5C'
    | '\u1E5E'
    | '\u1E60'
    | '\u1E62'
    | '\u1E64'
    | '\u1E66'
    | '\u1E68'
    | '\u1E6A'
    | '\u1E6C'
    | '\u1E6E'
    | '\u1E70'
    | '\u1E72'
    | '\u1E74'
    | '\u1E76'
    | '\u1E78'
    | '\u1E7A'
    | '\u1E7C'
    | '\u1E7E'
    | '\u1E80'
    | '\u1E82'
    | '\u1E84'
    | '\u1E86'
    | '\u1E88'
    | '\u1E8A'
    | '\u1E8C'
    | '\u1E8E'
    | '\u1E90'
    | '\u1E92'
    | '\u1E94'
    | '\u1E9E'
    | '\u1EA0'
    | '\u1EA2'
    | '\u1EA4'
    | '\u1EA6'
    | '\u1EA8'
    | '\u1EAA'
    | '\u1EAC'
    | '\u1EAE'
    | '\u1EB0'
    | '\u1EB2'
    | '\u1EB4'
    | '\u1EB6'
    | '\u1EB8'
    | '\u1EBA'
    | '\u1EBC'
    | '\u1EBE'
    | '\u1EC0'
    | '\u1EC2'
    | '\u1EC4'
    | '\u1EC6'
    | '\u1EC8'
    | '\u1ECA'
    | '\u1ECC'
    | '\u1ECE'
    | '\u1ED0'
    | '\u1ED2'
    | '\u1ED4'
    | '\u1ED6'
    | '\u1ED8'
    | '\u1EDA'
    | '\u1EDC'
    | '\u1EDE'
    | '\u1EE0'
    | '\u1EE2'
    | '\u1EE4'
    | '\u1EE6'
    | '\u1EE8'
    | '\u1EEA'
    | '\u1EEC'
    | '\u1EEE'
    | '\u1EF0'
    | '\u1EF2'
    | '\u1EF4'
    | '\u1EF6'
    | '\u1EF8'
    | '\u1EFA'
    | '\u1EFC'
    | '\u1EFE'
    | '\u1F08' .. '\u1F0F'
    | '\u1F18' .. '\u1F1D'
    | '\u1F28' .. '\u1F2F'
    | '\u1F38' .. '\u1F3F'
    | '\u1F48' .. '\u1F4D'
    | '\u1F59'
    | '\u1F5B'
    | '\u1F5D'
    | '\u1F5F'
    | '\u1F68' .. '\u1F6F'
    | '\u1FB8' .. '\u1FBB'
    | '\u1FC8' .. '\u1FCB'
    | '\u1FD8' .. '\u1FDB'
    | '\u1FE8' .. '\u1FEC'
    | '\u1FF8' .. '\u1FFB'
    | '\u2102'
    | '\u2107'
    | '\u210B' .. '\u210D'
    | '\u2110' .. '\u2112'
    | '\u2115'
    | '\u2119' .. '\u211D'
    | '\u2124'
    | '\u2126'
    | '\u2128'
    | '\u212A' .. '\u212D'
    | '\u2130' .. '\u2133'
    | '\u213E' .. '\u213F'
    | '\u2145'
    | '\u2183'
    | '\u2C00' .. '\u2C2E'
    | '\u2C60'
    | '\u2C62' .. '\u2C64'
    | '\u2C67'
    | '\u2C69'
    | '\u2C6B'
    | '\u2C6D' .. '\u2C70'
    | '\u2C72'
    | '\u2C75'
    | '\u2C7E' .. '\u2C80'
    | '\u2C82'
    | '\u2C84'
    | '\u2C86'
    | '\u2C88'
    | '\u2C8A'
    | '\u2C8C'
    | '\u2C8E'
    | '\u2C90'
    | '\u2C92'
    | '\u2C94'
    | '\u2C96'
    | '\u2C98'
    | '\u2C9A'
    | '\u2C9C'
    | '\u2C9E'
    | '\u2CA0'
    | '\u2CA2'
    | '\u2CA4'
    | '\u2CA6'
    | '\u2CA8'
    | '\u2CAA'
    | '\u2CAC'
    | '\u2CAE'
    | '\u2CB0'
    | '\u2CB2'
    | '\u2CB4'
    | '\u2CB6'
    | '\u2CB8'
    | '\u2CBA'
    | '\u2CBC'
    | '\u2CBE'
    | '\u2CC0'
    | '\u2CC2'
    | '\u2CC4'
    | '\u2CC6'
    | '\u2CC8'
    | '\u2CCA'
    | '\u2CCC'
    | '\u2CCE'
    | '\u2CD0'
    | '\u2CD2'
    | '\u2CD4'
    | '\u2CD6'
    | '\u2CD8'
    | '\u2CDA'
    | '\u2CDC'
    | '\u2CDE'
    | '\u2CE0'
    | '\u2CE2'
    | '\u2CEB'
    | '\u2CED'
    | '\u2CF2'
    | '\uA640'
    | '\uA642'
    | '\uA644'
    | '\uA646'
    | '\uA648'
    | '\uA64A'
    | '\uA64C'
    | '\uA64E'
    | '\uA650'
    | '\uA652'
    | '\uA654'
    | '\uA656'
    | '\uA658'
    | '\uA65A'
    | '\uA65C'
    | '\uA65E'
    | '\uA660'
    | '\uA662'
    | '\uA664'
    | '\uA666'
    | '\uA668'
    | '\uA66A'
    | '\uA66C'
    | '\uA680'
    | '\uA682'
    | '\uA684'
    | '\uA686'
    | '\uA688'
    | '\uA68A'
    | '\uA68C'
    | '\uA68E'
    | '\uA690'
    | '\uA692'
    | '\uA694'
    | '\uA696'
    | '\uA698'
    | '\uA69A'
    | '\uA722'
    | '\uA724'
    | '\uA726'
    | '\uA728'
    | '\uA72A'
    | '\uA72C'
    | '\uA72E'
    | '\uA732'
    | '\uA734'
    | '\uA736'
    | '\uA738'
    | '\uA73A'
    | '\uA73C'
    | '\uA73E'
    | '\uA740'
    | '\uA742'
    | '\uA744'
    | '\uA746'
    | '\uA748'
    | '\uA74A'
    | '\uA74C'
    | '\uA74E'
    | '\uA750'
    | '\uA752'
    | '\uA754'
    | '\uA756'
    | '\uA758'
    | '\uA75A'
    | '\uA75C'
    | '\uA75E'
    | '\uA760'
    | '\uA762'
    | '\uA764'
    | '\uA766'
    | '\uA768'
    | '\uA76A'
    | '\uA76C'
    | '\uA76E'
    | '\uA779'
    | '\uA77B'
    | '\uA77D' .. '\uA77E'
    | '\uA780'
    | '\uA782'
    | '\uA784'
    | '\uA786'
    | '\uA78B'
    | '\uA78D'
    | '\uA790'
    | '\uA792'
    | '\uA796'
    | '\uA798'
    | '\uA79A'
    | '\uA79C'
    | '\uA79E'
    | '\uA7A0'
    | '\uA7A2'
    | '\uA7A4'
    | '\uA7A6'
    | '\uA7A8'
    | '\uA7AA' .. '\uA7AE'
    | '\uA7B0' .. '\uA7B4'
    | '\uA7B6'
    | '\uA7B8'
    | '\uA7BA'
    | '\uA7BC'
    | '\uA7BE'
    | '\uA7C2'
    | '\uA7C4' .. '\uA7C7'
    | '\uA7C9'
    | '\uA7F5'
    | '\uFF21' .. '\uFF3A'
    ;

fragment UnicodeLo
    : '\u00AA'
    | '\u00BA'
    | '\u01BB'
    | '\u01C0' .. '\u01C3'
    | '\u0294'
    | '\u05D0' .. '\u05EA'
    | '\u05EF' .. '\u05F2'
    | '\u0620' .. '\u063F'
    | '\u0641' .. '\u064A'
    | '\u066E' .. '\u066F'
    | '\u0671' .. '\u06D3'
    | '\u06D5'
    | '\u06EE' .. '\u06EF'
    | '\u06FA' .. '\u06FC'
    | '\u06FF'
    | '\u0710'
    | '\u0712' .. '\u072F'
    | '\u074D' .. '\u07A5'
    | '\u07B1'
    | '\u07CA' .. '\u07EA'
    | '\u0800' .. '\u0815'
    | '\u0840' .. '\u0858'
    | '\u0860' .. '\u086A'
    | '\u08A0' .. '\u08B4'
    | '\u08B6' .. '\u08C7'
    | '\u0904' .. '\u0939'
    | '\u093D'
    | '\u0950'
    | '\u0958' .. '\u0961'
    | '\u0972' .. '\u0980'
    | '\u0985' .. '\u098C'
    | '\u098F' .. '\u0990'
    | '\u0993' .. '\u09A8'
    | '\u09AA' .. '\u09B0'
    | '\u09B2'
    | '\u09B6' .. '\u09B9'
    | '\u09BD'
    | '\u09CE'
    | '\u09DC' .. '\u09DD'
    | '\u09DF' .. '\u09E1'
    | '\u09F0' .. '\u09F1'
    | '\u09FC'
    | '\u0A05' .. '\u0A0A'
    | '\u0A0F' .. '\u0A10'
    | '\u0A13' .. '\u0A28'
    | '\u0A2A' .. '\u0A30'
    | '\u0A32' .. '\u0A33'
    | '\u0A35' .. '\u0A36'
    | '\u0A38' .. '\u0A39'
    | '\u0A59' .. '\u0A5C'
    | '\u0A5E'
    | '\u0A72' .. '\u0A74'
    | '\u0A85' .. '\u0A8D'
    | '\u0A8F' .. '\u0A91'
    | '\u0A93' .. '\u0AA8'
    | '\u0AAA' .. '\u0AB0'
    | '\u0AB2' .. '\u0AB3'
    | '\u0AB5' .. '\u0AB9'
    | '\u0ABD'
    | '\u0AD0'
    | '\u0AE0' .. '\u0AE1'
    | '\u0AF9'
    | '\u0B05' .. '\u0B0C'
    | '\u0B0F' .. '\u0B10'
    | '\u0B13' .. '\u0B28'
    | '\u0B2A' .. '\u0B30'
    | '\u0B32' .. '\u0B33'
    | '\u0B35' .. '\u0B39'
    | '\u0B3D'
    | '\u0B5C' .. '\u0B5D'
    | '\u0B5F' .. '\u0B61'
    | '\u0B71'
    | '\u0B83'
    | '\u0B85' .. '\u0B8A'
    | '\u0B8E' .. '\u0B90'
    | '\u0B92' .. '\u0B95'
    | '\u0B99' .. '\u0B9A'
    | '\u0B9C'
    | '\u0B9E' .. '\u0B9F'
    | '\u0BA3' .. '\u0BA4'
    | '\u0BA8' .. '\u0BAA'
    | '\u0BAE' .. '\u0BB9'
    | '\u0BD0'
    | '\u0C05' .. '\u0C0C'
    | '\u0C0E' .. '\u0C10'
    | '\u0C12' .. '\u0C28'
    | '\u0C2A' .. '\u0C39'
    | '\u0C3D'
    | '\u0C58' .. '\u0C5A'
    | '\u0C60' .. '\u0C61'
    | '\u0C80'
    | '\u0C85' .. '\u0C8C'
    | '\u0C8E' .. '\u0C90'
    | '\u0C92' .. '\u0CA8'
    | '\u0CAA' .. '\u0CB3'
    | '\u0CB5' .. '\u0CB9'
    | '\u0CBD'
    | '\u0CDE'
    | '\u0CE0' .. '\u0CE1'
    | '\u0CF1' .. '\u0CF2'
    | '\u0D04' .. '\u0D0C'
    | '\u0D0E' .. '\u0D10'
    | '\u0D12' .. '\u0D3A'
    | '\u0D3D'
    | '\u0D4E'
    | '\u0D54' .. '\u0D56'
    | '\u0D5F' .. '\u0D61'
    | '\u0D7A' .. '\u0D7F'
    | '\u0D85' .. '\u0D96'
    | '\u0D9A' .. '\u0DB1'
    | '\u0DB3' .. '\u0DBB'
    | '\u0DBD'
    | '\u0DC0' .. '\u0DC6'
    | '\u0E01' .. '\u0E30'
    | '\u0E32' .. '\u0E33'
    | '\u0E40' .. '\u0E45'
    | '\u0E81' .. '\u0E82'
    | '\u0E84'
    | '\u0E86' .. '\u0E8A'
    | '\u0E8C' .. '\u0EA3'
    | '\u0EA5'
    | '\u0EA7' .. '\u0EB0'
    | '\u0EB2' .. '\u0EB3'
    | '\u0EBD'
    | '\u0EC0' .. '\u0EC4'
    | '\u0EDC' .. '\u0EDF'
    | '\u0F00'
    | '\u0F40' .. '\u0F47'
    | '\u0F49' .. '\u0F6C'
    | '\u0F88' .. '\u0F8C'
    | '\u1000' .. '\u102A'
    | '\u103F'
    | '\u1050' .. '\u1055'
    | '\u105A' .. '\u105D'
    | '\u1061'
    | '\u1065' .. '\u1066'
    | '\u106E' .. '\u1070'
    | '\u1075' .. '\u1081'
    | '\u108E'
    | '\u1100' .. '\u1248'
    | '\u124A' .. '\u124D'
    | '\u1250' .. '\u1256'
    | '\u1258'
    | '\u125A' .. '\u125D'
    | '\u1260' .. '\u1288'
    | '\u128A' .. '\u128D'
    | '\u1290' .. '\u12B0'
    | '\u12B2' .. '\u12B5'
    | '\u12B8' .. '\u12BE'
    | '\u12C0'
    | '\u12C2' .. '\u12C5'
    | '\u12C8' .. '\u12D6'
    | '\u12D8' .. '\u1310'
    | '\u1312' .. '\u1315'
    | '\u1318' .. '\u135A'
    | '\u1380' .. '\u138F'
    | '\u1401' .. '\u166C'
    | '\u166F' .. '\u167F'
    | '\u1681' .. '\u169A'
    | '\u16A0' .. '\u16EA'
    | '\u16F1' .. '\u16F8'
    | '\u1700' .. '\u170C'
    | '\u170E' .. '\u1711'
    | '\u1720' .. '\u1731'
    | '\u1740' .. '\u1751'
    | '\u1760' .. '\u176C'
    | '\u176E' .. '\u1770'
    | '\u1780' .. '\u17B3'
    | '\u17DC'
    | '\u1820' .. '\u1842'
    | '\u1844' .. '\u1878'
    | '\u1880' .. '\u1884'
    | '\u1887' .. '\u18A8'
    | '\u18AA'
    | '\u18B0' .. '\u18F5'
    | '\u1900' .. '\u191E'
    | '\u1950' .. '\u196D'
    | '\u1970' .. '\u1974'
    | '\u1980' .. '\u19AB'
    | '\u19B0' .. '\u19C9'
    | '\u1A00' .. '\u1A16'
    | '\u1A20' .. '\u1A54'
    | '\u1B05' .. '\u1B33'
    | '\u1B45' .. '\u1B4B'
    | '\u1B83' .. '\u1BA0'
    | '\u1BAE' .. '\u1BAF'
    | '\u1BBA' .. '\u1BE5'
    | '\u1C00' .. '\u1C23'
    | '\u1C4D' .. '\u1C4F'
    | '\u1C5A' .. '\u1C77'
    | '\u1CE9' .. '\u1CEC'
    | '\u1CEE' .. '\u1CF3'
    | '\u1CF5' .. '\u1CF6'
    | '\u1CFA'
    | '\u2135' .. '\u2138'
    | '\u2D30' .. '\u2D67'
    | '\u2D80' .. '\u2D96'
    | '\u2DA0' .. '\u2DA6'
    | '\u2DA8' .. '\u2DAE'
    | '\u2DB0' .. '\u2DB6'
    | '\u2DB8' .. '\u2DBE'
    | '\u2DC0' .. '\u2DC6'
    | '\u2DC8' .. '\u2DCE'
    | '\u2DD0' .. '\u2DD6'
    | '\u2DD8' .. '\u2DDE'
    | '\u3006'
    | '\u303C'
    | '\u3041' .. '\u3096'
    | '\u309F'
    | '\u30A1' .. '\u30FA'
    | '\u30FF'
    | '\u3105' .. '\u312F'
    | '\u3131' .. '\u318E'
    | '\u31A0' .. '\u31BF'
    | '\u31F0' .. '\u31FF'
    | '\u3400' .. '\u4DBF'
    | '\u4E00' .. '\u9FFC'
    | '\uA000' .. '\uA014'
    | '\uA016' .. '\uA48C'
    | '\uA4D0' .. '\uA4F7'
    | '\uA500' .. '\uA60B'
    | '\uA610' .. '\uA61F'
    | '\uA62A' .. '\uA62B'
    | '\uA66E'
    | '\uA6A0' .. '\uA6E5'
    | '\uA78F'
    | '\uA7F7'
    | '\uA7FB' .. '\uA801'
    | '\uA803' .. '\uA805'
    | '\uA807' .. '\uA80A'
    | '\uA80C' .. '\uA822'
    | '\uA840' .. '\uA873'
    | '\uA882' .. '\uA8B3'
    | '\uA8F2' .. '\uA8F7'
    | '\uA8FB'
    | '\uA8FD' .. '\uA8FE'
    | '\uA90A' .. '\uA925'
    | '\uA930' .. '\uA946'
    | '\uA960' .. '\uA97C'
    | '\uA984' .. '\uA9B2'
    | '\uA9E0' .. '\uA9E4'
    | '\uA9E7' .. '\uA9EF'
    | '\uA9FA' .. '\uA9FE'
    | '\uAA00' .. '\uAA28'
    | '\uAA40' .. '\uAA42'
    | '\uAA44' .. '\uAA4B'
    | '\uAA60' .. '\uAA6F'
    | '\uAA71' .. '\uAA76'
    | '\uAA7A'
    | '\uAA7E' .. '\uAAAF'
    | '\uAAB1'
    | '\uAAB5' .. '\uAAB6'
    | '\uAAB9' .. '\uAABD'
    | '\uAAC0'
    | '\uAAC2'
    | '\uAADB' .. '\uAADC'
    | '\uAAE0' .. '\uAAEA'
    | '\uAAF2'
    | '\uAB01' .. '\uAB06'
    | '\uAB09' .. '\uAB0E'
    | '\uAB11' .. '\uAB16'
    | '\uAB20' .. '\uAB26'
    | '\uAB28' .. '\uAB2E'
    | '\uABC0' .. '\uABE2'
    | '\uAC00' .. '\uD7A3'
    | '\uD7B0' .. '\uD7C6'
    | '\uD7CB' .. '\uD7FB'
    | '\uF900' .. '\uFA6D'
    | '\uFA70' .. '\uFAD9'
    | '\uFB1D'
    | '\uFB1F' .. '\uFB28'
    | '\uFB2A' .. '\uFB36'
    | '\uFB38' .. '\uFB3C'
    | '\uFB3E'
    | '\uFB40' .. '\uFB41'
    | '\uFB43' .. '\uFB44'
    | '\uFB46' .. '\uFBB1'
    | '\uFBD3' .. '\uFD3D'
    | '\uFD50' .. '\uFD8F'
    | '\uFD92' .. '\uFDC7'
    | '\uFDF0' .. '\uFDFB'
    | '\uFE70' .. '\uFE74'
    | '\uFE76' .. '\uFEFC'
    | '\uFF66' .. '\uFF6F'
    | '\uFF71' .. '\uFF9D'
    | '\uFFA0' .. '\uFFBE'
    | '\uFFC2' .. '\uFFC7'
    | '\uFFCA' .. '\uFFCF'
    | '\uFFD2' .. '\uFFD7'
    | '\uFFDA' .. '\uFFDC'
    ;

fragment UnicodeLl
    : '\u0061' .. '\u007A'
    | '\u00B5'
    | '\u00DF' .. '\u00F6'
    | '\u00F8' .. '\u00FF'
    | '\u0101'
    | '\u0103'
    | '\u0105'
    | '\u0107'
    | '\u0109'
    | '\u010B'
    | '\u010D'
    | '\u010F'
    | '\u0111'
    | '\u0113'
    | '\u0115'
    | '\u0117'
    | '\u0119'
    | '\u011B'
    | '\u011D'
    | '\u011F'
    | '\u0121'
    | '\u0123'
    | '\u0125'
    | '\u0127'
    | '\u0129'
    | '\u012B'
    | '\u012D'
    | '\u012F'
    | '\u0131'
    | '\u0133'
    | '\u0135'
    | '\u0137' .. '\u0138'
    | '\u013A'
    | '\u013C'
    | '\u013E'
    | '\u0140'
    | '\u0142'
    | '\u0144'
    | '\u0146'
    | '\u0148' .. '\u0149'
    | '\u014B'
    | '\u014D'
    | '\u014F'
    | '\u0151'
    | '\u0153'
    | '\u0155'
    | '\u0157'
    | '\u0159'
    | '\u015B'
    | '\u015D'
    | '\u015F'
    | '\u0161'
    | '\u0163'
    | '\u0165'
    | '\u0167'
    | '\u0169'
    | '\u016B'
    | '\u016D'
    | '\u016F'
    | '\u0171'
    | '\u0173'
    | '\u0175'
    | '\u0177'
    | '\u017A'
    | '\u017C'
    | '\u017E' .. '\u0180'
    | '\u0183'
    | '\u0185'
    | '\u0188'
    | '\u018C' .. '\u018D'
    | '\u0192'
    | '\u0195'
    | '\u0199' .. '\u019B'
    | '\u019E'
    | '\u01A1'
    | '\u01A3'
    | '\u01A5'
    | '\u01A8'
    | '\u01AA' .. '\u01AB'
    | '\u01AD'
    | '\u01B0'
    | '\u01B4'
    | '\u01B6'
    | '\u01B9' .. '\u01BA'
    | '\u01BD' .. '\u01BF'
    | '\u01C6'
    | '\u01C9'
    | '\u01CC'
    | '\u01CE'
    | '\u01D0'
    | '\u01D2'
    | '\u01D4'
    | '\u01D6'
    | '\u01D8'
    | '\u01DA'
    | '\u01DC' .. '\u01DD'
    | '\u01DF'
    | '\u01E1'
    | '\u01E3'
    | '\u01E5'
    | '\u01E7'
    | '\u01E9'
    | '\u01EB'
    | '\u01ED'
    | '\u01EF' .. '\u01F0'
    | '\u01F3'
    | '\u01F5'
    | '\u01F9'
    | '\u01FB'
    | '\u01FD'
    | '\u01FF'
    | '\u0201'
    | '\u0203'
    | '\u0205'
    | '\u0207'
    | '\u0209'
    | '\u020B'
    | '\u020D'
    | '\u020F'
    | '\u0211'
    | '\u0213'
    | '\u0215'
    | '\u0217'
    | '\u0219'
    | '\u021B'
    | '\u021D'
    | '\u021F'
    | '\u0221'
    | '\u0223'
    | '\u0225'
    | '\u0227'
    | '\u0229'
    | '\u022B'
    | '\u022D'
    | '\u022F'
    | '\u0231'
    | '\u0233' .. '\u0239'
    | '\u023C'
    | '\u023F' .. '\u0240'
    | '\u0242'
    | '\u0247'
    | '\u0249'
    | '\u024B'
    | '\u024D'
    | '\u024F' .. '\u0293'
    | '\u0295' .. '\u02AF'
    | '\u0371'
    | '\u0373'
    | '\u0377'
    | '\u037B' .. '\u037D'
    | '\u0390'
    | '\u03AC' .. '\u03CE'
    | '\u03D0' .. '\u03D1'
    | '\u03D5' .. '\u03D7'
    | '\u03D9'
    | '\u03DB'
    | '\u03DD'
    | '\u03DF'
    | '\u03E1'
    | '\u03E3'
    | '\u03E5'
    | '\u03E7'
    | '\u03E9'
    | '\u03EB'
    | '\u03ED'
    | '\u03EF' .. '\u03F3'
    | '\u03F5'
    | '\u03F8'
    | '\u03FB' .. '\u03FC'
    | '\u0430' .. '\u045F'
    | '\u0461'
    | '\u0463'
    | '\u0465'
    | '\u0467'
    | '\u0469'
    | '\u046B'
    | '\u046D'
    | '\u046F'
    | '\u0471'
    | '\u0473'
    | '\u0475'
    | '\u0477'
    | '\u0479'
    | '\u047B'
    | '\u047D'
    | '\u047F'
    | '\u0481'
    | '\u048B'
    | '\u048D'
    | '\u048F'
    | '\u0491'
    | '\u0493'
    | '\u0495'
    | '\u0497'
    | '\u0499'
    | '\u049B'
    | '\u049D'
    | '\u049F'
    | '\u04A1'
    | '\u04A3'
    | '\u04A5'
    | '\u04A7'
    | '\u04A9'
    | '\u04AB'
    | '\u04AD'
    | '\u04AF'
    | '\u04B1'
    | '\u04B3'
    | '\u04B5'
    | '\u04B7'
    | '\u04B9'
    | '\u04BB'
    | '\u04BD'
    | '\u04BF'
    | '\u04C2'
    | '\u04C4'
    | '\u04C6'
    | '\u04C8'
    | '\u04CA'
    | '\u04CC'
    | '\u04CE' .. '\u04CF'
    | '\u04D1'
    | '\u04D3'
    | '\u04D5'
    | '\u04D7'
    | '\u04D9'
    | '\u04DB'
    | '\u04DD'
    | '\u04DF'
    | '\u04E1'
    | '\u04E3'
    | '\u04E5'
    | '\u04E7'
    | '\u04E9'
    | '\u04EB'
    | '\u04ED'
    | '\u04EF'
    | '\u04F1'
    | '\u04F3'
    | '\u04F5'
    | '\u04F7'
    | '\u04F9'
    | '\u04FB'
    | '\u04FD'
    | '\u04FF'
    | '\u0501'
    | '\u0503'
    | '\u0505'
    | '\u0507'
    | '\u0509'
    | '\u050B'
    | '\u050D'
    | '\u050F'
    | '\u0511'
    | '\u0513'
    | '\u0515'
    | '\u0517'
    | '\u0519'
    | '\u051B'
    | '\u051D'
    | '\u051F'
    | '\u0521'
    | '\u0523'
    | '\u0525'
    | '\u0527'
    | '\u0529'
    | '\u052B'
    | '\u052D'
    | '\u052F'
    | '\u0560' .. '\u0588'
    | '\u10D0' .. '\u10FA'
    | '\u10FD' .. '\u10FF'
    | '\u13F8' .. '\u13FD'
    | '\u1C80' .. '\u1C88'
    | '\u1D00' .. '\u1D2B'
    | '\u1D6B' .. '\u1D77'
    | '\u1D79' .. '\u1D9A'
    | '\u1E01'
    | '\u1E03'
    | '\u1E05'
    | '\u1E07'
    | '\u1E09'
    | '\u1E0B'
    | '\u1E0D'
    | '\u1E0F'
    | '\u1E11'
    | '\u1E13'
    | '\u1E15'
    | '\u1E17'
    | '\u1E19'
    | '\u1E1B'
    | '\u1E1D'
    | '\u1E1F'
    | '\u1E21'
    | '\u1E23'
    | '\u1E25'
    | '\u1E27'
    | '\u1E29'
    | '\u1E2B'
    | '\u1E2D'
    | '\u1E2F'
    | '\u1E31'
    | '\u1E33'
    | '\u1E35'
    | '\u1E37'
    | '\u1E39'
    | '\u1E3B'
    | '\u1E3D'
    | '\u1E3F'
    | '\u1E41'
    | '\u1E43'
    | '\u1E45'
    | '\u1E47'
    | '\u1E49'
    | '\u1E4B'
    | '\u1E4D'
    | '\u1E4F'
    | '\u1E51'
    | '\u1E53'
    | '\u1E55'
    | '\u1E57'
    | '\u1E59'
    | '\u1E5B'
    | '\u1E5D'
    | '\u1E5F'
    | '\u1E61'
    | '\u1E63'
    | '\u1E65'
    | '\u1E67'
    | '\u1E69'
    | '\u1E6B'
    | '\u1E6D'
    | '\u1E6F'
    | '\u1E71'
    | '\u1E73'
    | '\u1E75'
    | '\u1E77'
    | '\u1E79'
    | '\u1E7B'
    | '\u1E7D'
    | '\u1E7F'
    | '\u1E81'
    | '\u1E83'
    | '\u1E85'
    | '\u1E87'
    | '\u1E89'
    | '\u1E8B'
    | '\u1E8D'
    | '\u1E8F'
    | '\u1E91'
    | '\u1E93'
    | '\u1E95' .. '\u1E9D'
    | '\u1E9F'
    | '\u1EA1'
    | '\u1EA3'
    | '\u1EA5'
    | '\u1EA7'
    | '\u1EA9'
    | '\u1EAB'
    | '\u1EAD'
    | '\u1EAF'
    | '\u1EB1'
    | '\u1EB3'
    | '\u1EB5'
    | '\u1EB7'
    | '\u1EB9'
    | '\u1EBB'
    | '\u1EBD'
    | '\u1EBF'
    | '\u1EC1'
    | '\u1EC3'
    | '\u1EC5'
    | '\u1EC7'
    | '\u1EC9'
    | '\u1ECB'
    | '\u1ECD'
    | '\u1ECF'
    | '\u1ED1'
    | '\u1ED3'
    | '\u1ED5'
    | '\u1ED7'
    | '\u1ED9'
    | '\u1EDB'
    | '\u1EDD'
    | '\u1EDF'
    | '\u1EE1'
    | '\u1EE3'
    | '\u1EE5'
    | '\u1EE7'
    | '\u1EE9'
    | '\u1EEB'
    | '\u1EED'
    | '\u1EEF'
    | '\u1EF1'
    | '\u1EF3'
    | '\u1EF5'
    | '\u1EF7'
    | '\u1EF9'
    | '\u1EFB'
    | '\u1EFD'
    | '\u1EFF' .. '\u1F07'
    | '\u1F10' .. '\u1F15'
    | '\u1F20' .. '\u1F27'
    | '\u1F30' .. '\u1F37'
    | '\u1F40' .. '\u1F45'
    | '\u1F50' .. '\u1F57'
    | '\u1F60' .. '\u1F67'
    | '\u1F70' .. '\u1F7D'
    | '\u1F80' .. '\u1F87'
    | '\u1F90' .. '\u1F97'
    | '\u1FA0' .. '\u1FA7'
    | '\u1FB0' .. '\u1FB4'
    | '\u1FB6' .. '\u1FB7'
    | '\u1FBE'
    | '\u1FC2' .. '\u1FC4'
    | '\u1FC6' .. '\u1FC7'
    | '\u1FD0' .. '\u1FD3'
    | '\u1FD6' .. '\u1FD7'
    | '\u1FE0' .. '\u1FE7'
    | '\u1FF2' .. '\u1FF4'
    | '\u1FF6' .. '\u1FF7'
    | '\u210A'
    | '\u210E' .. '\u210F'
    | '\u2113'
    | '\u212F'
    | '\u2134'
    | '\u2139'
    | '\u213C' .. '\u213D'
    | '\u2146' .. '\u2149'
    | '\u214E'
    | '\u2184'
    | '\u2C30' .. '\u2C5E'
    | '\u2C61'
    | '\u2C65' .. '\u2C66'
    | '\u2C68'
    | '\u2C6A'
    | '\u2C6C'
    | '\u2C71'
    | '\u2C73' .. '\u2C74'
    | '\u2C76' .. '\u2C7B'
    | '\u2C81'
    | '\u2C83'
    | '\u2C85'
    | '\u2C87'
    | '\u2C89'
    | '\u2C8B'
    | '\u2C8D'
    | '\u2C8F'
    | '\u2C91'
    | '\u2C93'
    | '\u2C95'
    | '\u2C97'
    | '\u2C99'
    | '\u2C9B'
    | '\u2C9D'
    | '\u2C9F'
    | '\u2CA1'
    | '\u2CA3'
    | '\u2CA5'
    | '\u2CA7'
    | '\u2CA9'
    | '\u2CAB'
    | '\u2CAD'
    | '\u2CAF'
    | '\u2CB1'
    | '\u2CB3'
    | '\u2CB5'
    | '\u2CB7'
    | '\u2CB9'
    | '\u2CBB'
    | '\u2CBD'
    | '\u2CBF'
    | '\u2CC1'
    | '\u2CC3'
    | '\u2CC5'
    | '\u2CC7'
    | '\u2CC9'
    | '\u2CCB'
    | '\u2CCD'
    | '\u2CCF'
    | '\u2CD1'
    | '\u2CD3'
    | '\u2CD5'
    | '\u2CD7'
    | '\u2CD9'
    | '\u2CDB'
    | '\u2CDD'
    | '\u2CDF'
    | '\u2CE1'
    | '\u2CE3' .. '\u2CE4'
    | '\u2CEC'
    | '\u2CEE'
    | '\u2CF3'
    | '\u2D00' .. '\u2D25'
    | '\u2D27'
    | '\u2D2D'
    | '\uA641'
    | '\uA643'
    | '\uA645'
    | '\uA647'
    | '\uA649'
    | '\uA64B'
    | '\uA64D'
    | '\uA64F'
    | '\uA651'
    | '\uA653'
    | '\uA655'
    | '\uA657'
    | '\uA659'
    | '\uA65B'
    | '\uA65D'
    | '\uA65F'
    | '\uA661'
    | '\uA663'
    | '\uA665'
    | '\uA667'
    | '\uA669'
    | '\uA66B'
    | '\uA66D'
    | '\uA681'
    | '\uA683'
    | '\uA685'
    | '\uA687'
    | '\uA689'
    | '\uA68B'
    | '\uA68D'
    | '\uA68F'
    | '\uA691'
    | '\uA693'
    | '\uA695'
    | '\uA697'
    | '\uA699'
    | '\uA69B'
    | '\uA723'
    | '\uA725'
    | '\uA727'
    | '\uA729'
    | '\uA72B'
    | '\uA72D'
    | '\uA72F' .. '\uA731'
    | '\uA733'
    | '\uA735'
    | '\uA737'
    | '\uA739'
    | '\uA73B'
    | '\uA73D'
    | '\uA73F'
    | '\uA741'
    | '\uA743'
    | '\uA745'
    | '\uA747'
    | '\uA749'
    | '\uA74B'
    | '\uA74D'
    | '\uA74F'
    | '\uA751'
    | '\uA753'
    | '\uA755'
    | '\uA757'
    | '\uA759'
    | '\uA75B'
    | '\uA75D'
    | '\uA75F'
    | '\uA761'
    | '\uA763'
    | '\uA765'
    | '\uA767'
    | '\uA769'
    | '\uA76B'
    | '\uA76D'
    | '\uA76F'
    | '\uA771' .. '\uA778'
    | '\uA77A'
    | '\uA77C'
    | '\uA77F'
    | '\uA781'
    | '\uA783'
    | '\uA785'
    | '\uA787'
    | '\uA78C'
    | '\uA78E'
    | '\uA791'
    | '\uA793' .. '\uA795'
    | '\uA797'
    | '\uA799'
    | '\uA79B'
    | '\uA79D'
    | '\uA79F'
    | '\uA7A1'
    | '\uA7A3'
    | '\uA7A5'
    | '\uA7A7'
    | '\uA7A9'
    | '\uA7AF'
    | '\uA7B5'
    | '\uA7B7'
    | '\uA7B9'
    | '\uA7BB'
    | '\uA7BD'
    | '\uA7BF'
    | '\uA7C3'
    | '\uA7C8'
    | '\uA7CA'
    | '\uA7F6'
    | '\uA7FA'
    | '\uAB30' .. '\uAB5A'
    | '\uAB60' .. '\uAB68'
    | '\uAB70' .. '\uABBF'
    | '\uFB00' .. '\uFB06'
    | '\uFB13' .. '\uFB17'
    | '\uFF41' .. '\uFF5A'
    ;
    