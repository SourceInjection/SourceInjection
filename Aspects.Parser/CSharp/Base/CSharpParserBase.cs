using Antlr4.Runtime;
using Aspects.Parsers.CSharp.Generated;

namespace Aspects.Parsers.CSharp.Base
{
    public abstract class CSharpParserBase(ITokenStream input) : Parser(input)
    {
        protected bool IsLocalVariableDeclaration()
        {
            if (Context is not CSharpParser.Local_variable_declarationContext localVarDecl)
                return true;
            var localVariableType = localVarDecl.local_variable_type();
            if (localVariableType == null)
                return true;
            if (localVariableType.GetText() == "var")
                return false;
            return true;
        }
    }
}