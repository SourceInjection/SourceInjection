using Antlr4.Runtime.Misc;
using Antlr4.Runtime;
using Aspects.Parsers.CSharp.Exceptions;

namespace Aspects.Parsers.CSharp.Listeners
{
    internal class ThrowExceptionListener : IAntlrErrorListener<IToken>
    {
        public void SyntaxError(
            [NotNull] IRecognizer recognizer,
            [Nullable] IToken offendingSymbol,
            int line,
            int charPositionInLine,
            [NotNull] string msg,
            [Nullable] RecognitionException e)
        {
            throw new MalformedCodeException($"Syntax error at line {line} column {charPositionInLine}: {msg}", e);
        }
    }
}
