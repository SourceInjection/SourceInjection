using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Aspects.Util
{
    internal static class SyntaxNodeExtensions
    {
        /// <summary>
        /// Extracts the name of the <see cref="TypeDeclarationSyntax"/> with generic type parameters.
        /// </summary>
        /// <param name="node">The <see cref="TypeDeclarationSyntax"/> for which the name and the generic type parameters are extracted.</param>
        /// <returns>The name with generic type parameters.</returns>
        public static string NameWithGenericParameters(this TypeDeclarationSyntax node)
        {
            if (node.TypeParameterList is null)
                return node.Identifier.Text;

            var typeDec = Declaration(node);
            return typeDec.Substring(typeDec.IndexOf(node.Identifier.Text)).Trim();
        }

        /// <summary>
        /// Gets the full name of the <see cref="TypeDeclarationSyntax"/> with generic type parameters.
        /// </summary>
        /// <param name="node">The <see cref="TypeDeclarationSyntax"/> for which the full name and the generic type parameters are extracted.</param>
        /// <returns>The full name with generic type parameters.</returns>
        public static string FullNameWithGenericParameters(this TypeDeclarationSyntax node)
        {
            var name = node.NameWithGenericParameters();

            while (node.Parent is TypeDeclarationSyntax type)
            {
                name = $"{type.NameWithGenericParameters()}.{name}";
                node = type;
            }

            NamespaceDeclarationSyntax ns = node.Parent as NamespaceDeclarationSyntax;
            while(ns != null)
            {
                name = $"{ns.Name}.{name}";
                ns = ns.Parent as NamespaceDeclarationSyntax;
            }

            return name;
        }

        /// <summary>
        /// Extracts the declaration of the <see cref="TypeDeclarationSyntax"/>.<br/>
        /// e.g. "public partial class MyClass" or 
        /// "public partial readonly struct MyGenericStruct<![CDATA[<]]>T<![CDATA[>]]>".<br/>
        /// Base class, implemented interfaces and attributes are not included.
        /// </summary>
        /// <param name="node">The node for which the declaration is extracted.</param>
        /// <returns>The declaration of the <see cref="TypeDeclarationSyntax"/>.</returns>
        public static string Declaration(this TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf('{'));
            nodeText = RemoveAttributes(nodeText).Trim();
            nodeText = RemoveCompilerDirectives(nodeText).Trim();

            if (nodeText.Contains('>'))
                return nodeText.Substring(0, nodeText.LastIndexOf('>') + 1);
            return nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length);
        }

        /// <summary>
        /// Checks if the type declaration has defined #nullable disable
        /// </summary>
        /// <param name="node">The node to be checked.</param>
        /// <returns><see langword="true"/> if the node has a #nullable disable restriction else <see langword="false"/></returns>
        public static bool HasNullableDisabledDirective(this TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text));
            nodeText = RemoveAttributes(nodeText);

            return nodeText.Split('\n')
                .Select(l => l.TrimStart())
                .Any(l => l.StartsWith("#nullable") && l.Contains("disable"));
        }

        /// <summary>
        /// Checks if the <see cref="TypeDeclarationSyntax"/> has a <see cref="partial"/> modifier.
        /// </summary>
        /// <param name="typeDeclaration">The <see cref="TypeDeclarationSyntax"/> wich is checked.</param>
        /// <returns><see langword="true"/> if the <see cref="TypeDeclarationSyntax"/> has a partial modifier else <see langword="false"/>.</returns>
        public static bool HasPartialModifier(this TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Modifiers
                .Any(m => m.IsKind(SyntaxKind.PartialKeyword));
        }

        /// <summary>
        /// Checks if the <see cref="PropertyDeclarationSyntax"/> counts as a data member.<br/>
        /// A Data Member matches the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The <see cref="PropertyDeclarationSyntax"/> 
        /// which is scanned for the linked <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="PropertyDeclarationSyntax"/> is a data member else <see langword="false"/>.</returns>
        public static bool IsDataMember(this PropertyDeclarationSyntax propertySyntax)
        {
            return IsDataMember(propertySyntax, GetReturnedIdentifier(propertySyntax));
        }

        /// <summary>
        /// Tries to get <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/> 
        /// which is linked with the property in the get method.<br/>
        /// Matches the following property definition grammer:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The <see cref="PropertyDeclarationSyntax"/> 
        /// which is scanned for the linked <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/>.</param>
        /// <param name="identifier">The resulting <see cref="SyntaxToken"/>.</param>
        /// <returns><see langword="true"/> if the property is linked with an <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/> else <see langword="false"/>.</returns>
        public static bool TryGetReturnedIdentifier(this PropertyDeclarationSyntax propertySyntax, out SyntaxToken identifier)
        {
            identifier = GetReturnedIdentifier(propertySyntax);
            return identifier.IsKind(SyntaxKind.IdentifierToken);
        }

        /// <summary>
        /// Gets the <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/> 
        /// which is linked with the property in the get method.<br/>
        /// Matches the following property definition grammer:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </summary>
        /// <param name="propertySyntax">The <see cref="PropertyDeclarationSyntax"/> 
        /// which is scanned for the linked <see cref="SyntaxToken"/> with kind <see cref="SyntaxKind.IdentifierToken"/>.</param>
        /// <returns>The <see cref="SyntaxToken"/> of kind <see cref="SyntaxKind.IdentifierToken"/>.<br/>default(<see cref="SyntaxToken"/>) if not found.</returns>
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

        private static string RemoveCompilerDirectives(string code)
        {
            var start = code.IndexOf('#');

            while (start >= 0)
            {
                var end = code.IndexOf('\n', start + 1);
                if (end < 0)
                    break;
                code = code.Substring(0, start) + code.Substring(end + 1);
                start = code.IndexOf('#');
            }
            return code;
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
