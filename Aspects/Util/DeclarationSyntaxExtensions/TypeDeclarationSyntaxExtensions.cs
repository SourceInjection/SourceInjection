using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;

namespace Aspects.Util.DeclarationSyntaxExtensions
{
    internal static class TypeDeclarationSyntaxExtensions
    {
        public static string NameWithGenericParameters(this TypeDeclarationSyntax node)
        {
            if (node.TypeParameterList is null)
                return node.Identifier.Text;

            var typeDec = node.Declaration();
            return typeDec.Substring(typeDec.IndexOf(node.Identifier.Text)).Trim();
        }

        public static string FullNameWithGenericParameters(this TypeDeclarationSyntax node)
        {
            var name = node.NameWithGenericParameters();

            while (node.Parent is TypeDeclarationSyntax type)
            {
                name = $"{type.NameWithGenericParameters()}.{name}";
                node = type;
            }

            NamespaceDeclarationSyntax ns = node.Parent as NamespaceDeclarationSyntax;
            while (ns != null)
            {
                name = $"{ns.Name}.{name}";
                ns = ns.Parent as NamespaceDeclarationSyntax;
            }

            return name;
        }

        public static string Declaration(this TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf('{'));
            nodeText = RemoveAttributes(nodeText).Trim();
            nodeText = RemoveCompilerDirectives(nodeText).Trim();

            var idx = nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length;
            while(idx < nodeText.Length)
            {
                if (nodeText[idx] == '(' || nodeText[idx] == '{')
                    break;

                if (nodeText[idx] == '<')
                    return nodeText.Substring(0, nodeText.IndexOf('>', idx + 1) + 1);
            }
            return nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length);
        }

        public static bool HasNullableDisabledDirective(this TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();
            nodeText = nodeText.Substring(0, nodeText.IndexOf(node.Identifier.Text));
            nodeText = RemoveAttributes(nodeText);

            return nodeText.Split('\n')
                .Select(l => l.TrimStart())
                .Any(l => l.StartsWith("#nullable") && l.Contains("disable"));
        }

        public static bool HasPartialModifier(this TypeDeclarationSyntax typeDeclaration)
        {
            return typeDeclaration.Modifiers
                .Any(m => m.IsKind(SyntaxKind.PartialKeyword));
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
    }
}
