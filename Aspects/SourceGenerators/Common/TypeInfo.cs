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
            Declaration = Common.Declaration.GetText(syntaxNode);
            Name = Common.Declaration.GetName(syntaxNode);
        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public string Declaration { get; }

        public string Name { get; }

        public IEnumerable<ISymbol> Members => Symbol.GetMembers();

        public IEnumerable<ISymbol> MembersWith<T>() where T : Attribute
        {
            var name = typeof(T).FullName;
            return Members.Where(m => m.GetAttributes().Any(a => a.AttributeClass.ToDisplayString() == name));
        }
    }
}
