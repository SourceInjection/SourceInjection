

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class SourceCode
    {
        public static IEnumerable<string> GetLinkedFields(TypeInfo typeInfo)
        {
            var fields = new HashSet<string>(typeInfo.Fields().Where(f => CanBeLinked(f)).Select(f => f.Name));           

            var propertyNodes = typeInfo.SyntaxNode.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Select(pds => pds.DescendantTokens().SkipWhile(t => !t.IsKind(SyntaxKind.GetKeyword) && t.ValueText != "=>"))
                .Where(en => en.Any())
                .Select(en => GetLinkedField(en))
                .Where(f => !string.IsNullOrEmpty(f) && fields.Contains(f));

            return propertyNodes;
        }

        public static bool CanBeLinked(IFieldSymbol f)
        {
            return !f.IsConst && !f.IsStatic && !f.IsImplicitlyDeclared;
        }

        private static string GetLinkedField(IEnumerable<SyntaxToken> tokens)
        {
            var first = tokens.First();
            SyntaxToken token = default;

            if (first.ValueText == "=>")
                token = tokens.Skip(1).FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));
            else if (first.IsKind(SyntaxKind.GetKeyword))
            {
                var next = first.GetNextToken();
                if (next.ValueText == "=>")
                    token = tokens.Skip(2).FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));

                else if(next.ValueText == "{")
                {
                    token = tokens.Skip(2)
                        .SkipWhile(t => !t.IsKind(SyntaxKind.ReturnKeyword))
                        .FirstOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken));
                }
            }

            if (token.IsKind(SyntaxKind.IdentifierToken))
                return token.ValueText;
            return null;
        }
    }
}
