using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Aspects.CodeAnalysis
{
    public static class PropertyDeclarationSyntaxExtensions
    {

        public static bool IsDataMember(this PropertyDeclarationSyntax propertySyntax)
        {
            return IsDataMember(propertySyntax, propertySyntax.GetReturnedIdentifier());
        }

        public static bool TryGetReturnedIdentifier(this PropertyDeclarationSyntax propertySyntax, out SyntaxToken identifier)
        {
            identifier = propertySyntax.GetReturnedIdentifier();
            return identifier.IsKind(SyntaxKind.IdentifierToken);
        }

        public static SyntaxToken GetReturnedIdentifier(this PropertyDeclarationSyntax propertySyntax)
        {
            var token = propertySyntax.DescendantTokens()
                .FirstOrDefault(t => t.IsKind(SyntaxKind.GetKeyword) || t.ValueText == "=>");

            if (token.ValueText == "=>")
                token = token.GetNextToken();
            else if (token.IsKind(SyntaxKind.GetKeyword))
            {
                token = token.GetNextToken();
                if (token.ValueText == "=>")
                    token = token.GetNextToken();

                else if (token.ValueText == "{")
                    token = GetReturnedIdentifierWithinBlock(token);
            }

            if (token.IsKind(SyntaxKind.IdentifierToken) && HasValidFollower(token))
                return token;

            return default;
        }

        private static bool IsDataMember(PropertyDeclarationSyntax propertySyntax, SyntaxToken identifierToken)
        {
            return identifierToken.IsKind(SyntaxKind.IdentifierToken)
                || propertySyntax.DescendantTokens()
                    .FirstOrDefault(t => t.IsKind(SyntaxKind.GetKeyword))
                    .GetNextToken().ValueText == ";";
        }

        private static bool HasValidFollower(SyntaxToken token)
        {
            return token.GetNextToken().ValueText == ";" || token.GetNextToken().ValueText == "??";
        }

        private static SyntaxToken GetReturnedIdentifierWithinBlock(SyntaxToken token)
        {
            if (token.ValueText != "{")
                return default;

            token = token.GetNextToken();
            var depth = 1;
            var result = default(SyntaxToken);

            while (!token.IsKind(SyntaxKind.None) && depth > 0)
            {
                if (token.ValueText == "{")
                    depth++;
                else if (token.ValueText == "}")
                    depth--;
                else if (token.IsKind(SyntaxKind.ReturnKeyword))
                {
                    if (!result.IsKind(SyntaxKind.None))
                        return default;

                    result = token.GetNextToken();
                    if (!result.IsKind(SyntaxKind.IdentifierToken) || !HasValidFollower(result))
                        return default;
                }
                token = token.GetNextToken();
            }
            return result;
        }


    }
}
