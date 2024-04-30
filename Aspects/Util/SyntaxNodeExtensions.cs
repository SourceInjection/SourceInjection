using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Aspects.Util
{
    internal static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Extracts the name of the syntax node with generic type attributes.
        /// </summary>
        /// <param name="node">The node for which the name and the generic type arguments are extracted</param>
        /// <returns>The name with generic type attributes</returns>
        public static string NameWithGenericParameters(this TypeDeclarationSyntax node)
        {
            if (node.TypeParameterList is null)
                return node.Identifier.Text;

            var typeDec = Declaration(node);
            return typeDec.Substring(typeDec.IndexOf(node.Identifier.Text)).Trim();
        }

        /// <summary>
        /// Extracts the declaration of the syntax node.
        /// E.g. "public partial class MyClass" or "public readonly struct MyGenericStruct<![CDATA[<]]>T<![CDATA[>]]>".
        /// </summary>
        /// <param name="node">The node for which the declaration is extracted</param>
        /// <returns>The declaration of the type</returns>
        public static string Declaration(this TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf('{'));
            nodeText = RemoveAttributes(nodeText).Trim();

            if (nodeText.Contains('>'))
                return nodeText.Substring(0, nodeText.IndexOf('>') + 1);
            return nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length);
        }

        /// <summary>
        /// Checks if the type declaration has a partial modifier
        /// </summary>
        /// <param name="typeDeclaration">The node wich is checked</param>
        /// <returns>true if the node has a partial modifier else false</returns>
        public static bool HasPartialModifier(this TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Modifiers
                .Any(m => m.IsKind(SyntaxKind.PartialKeyword));
        }

        /// <summary>
        /// Checks if the property declaration is a data member.<br/>
        /// A Data Member matches the following syntax:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The property node which is scanned for the linked identifier</param>
        /// <returns>true if the property node is a data member else false</returns>
        public static bool IsDataMember(this PropertyDeclarationSyntax propertySyntax)
        {
            return IsDataMember(propertySyntax, GetReturnedIdentifier(propertySyntax));
        }

        /// <summary>
        /// Tries to get the syntax token which is linked wiith the property in the get method.<br/>
        /// Matches the following property definition grammer:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The property node which is scanned for the linked identifier</param>
        /// <param name="identifier">The resulting identifier token</param>
        /// <returns>true if the property is linked with an identifier else false</returns>
        public static bool TryGetReturnedIdentifier(this PropertyDeclarationSyntax propertySyntax, out SyntaxToken identifier)
        {
            identifier = GetReturnedIdentifier(propertySyntax);
            return identifier.IsKind(SyntaxKind.IdentifierToken);
        }

        /// <summary>
        /// Gets the identifier symbol which matches the following property definition grammer:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The property node which is scanned for the linked identifier</param>
        /// <returns>The <see cref="SyntaxToken"/> of the identifier.<br/><see cref="default"/> if not found.</returns>
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

        private static string RemoveAttributes(string text)
        {
            while (text.Contains('['))
            {
                (var idx, var length) = AttributeLocation(text);
                text = text.Remove(idx, length);
            }
            return text;
        }

        private static (int Idx, int Length) AttributeLocation(string text)
        {
            var idx = text.IndexOf('[');
            int length = 1;
            int nestLevel = 0;

            while (idx + length < text.Length && (nestLevel > 0 || text[idx + length] != ']'))
            {
                if (text[idx + length] == '[')
                    nestLevel++;
                else if (text[idx + length] == ']')
                    nestLevel--;
                length++;
            }
            if (idx + length < text.Length)
                length++;

            return (idx, length);
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
