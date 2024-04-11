using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal class TypeInfo
    {
        public TypeInfo(TypeDeclarationSyntax syntaxNode, INamedTypeSymbol typeSymbol)
        {
            SyntaxNode = syntaxNode;
            Symbol = typeSymbol;
            Declaration = TypeDeclaration(syntaxNode);
        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public string Declaration { get; }

        public IEnumerable<ISymbol> Members => Symbol.GetMembers();

        public IEnumerable<ISymbol> MembersWith<T>() where T : Attribute
        {
            var name = typeof(T).FullName;
            return Members.Where(m => m.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == name));
        }

        private static string TypeDeclaration(TypeDeclarationSyntax node)
        {
            var nodeText = node.GetText().ToString();

            int nameStart = 0;
            if (node.AttributeLists.Any())
                nameStart = node.AttributeLists.Sum(al => al.GetText().Length);

            int nameLength;
            if (node.TypeParameterList is null)
                nameLength = nodeText.IndexOf(node.Identifier.Text) + node.Identifier.Text.Length;
            else
                nameLength = nodeText.IndexOf('>') + 1;
            nameLength -= nameStart;

            return nodeText.Substring(nameStart, nameLength).Trim();
        }

        public override int GetHashCode()
        {
            return HashCodeCom
        }
    }
}
