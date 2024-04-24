using Aspects.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            HasPartialModifier = Common.Declaration.HasPartialModifier(syntaxNode);
            Declaration = Common.Declaration.GetText(syntaxNode);
            Name = Common.Declaration.GetName(syntaxNode);
        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool HasPartialModifier { get; }

        public string Declaration { get; }

        public string Name { get; }

        public IEnumerable<INamedTypeSymbol> OrderedInheritance(bool includeSelf = false)
        {
            var stack = new Stack<INamedTypeSymbol>();
            var sy = includeSelf ? Symbol : Symbol.BaseType;

            while (sy != null)
            {
                stack.Push(sy);
                sy = sy.BaseType;
            }

            while (stack.Count > 0)
                yield return stack.Pop();
        }

        public IEnumerable<ISymbol> Members(bool includeInherited = false)
        {
            if (!includeInherited)
                return Symbol.GetMembers();

            return OrderedInheritance(true).SelectMany(cl => cl.GetMembers());
        }

        public IEnumerable<IMethodSymbol> Methods(bool includeInherited = false)
        {
            return Members(includeInherited).OfType<IMethodSymbol>();
        }

        public IEnumerable<IFieldSymbol> Fields(bool includeInherited = false)
        {
            return Members(includeInherited).OfType<IFieldSymbol>();
        }

        public IEnumerable<IPropertySymbol> Properties(bool includeInherited = false)
        {
            return Members(includeInherited).OfType<IPropertySymbol>();
        }

        public IEnumerable<ISymbol> MembersWithAttributeOfType<T>(bool includeInherited = false)
        {
            return Members(includeInherited).Where(m => m.HasAttributeOfType<T>());
        }

        public IEnumerable<IFieldSymbol> FieldsWithAttributeOfType<T>(bool includeInherited = false)
        {
            return MembersWithAttributeOfType<T>(includeInherited).OfType<IFieldSymbol>();
        }

        public IEnumerable<IPropertySymbol> PropertiesWithAttributeOfType<T>(bool includeInherited = false)
        {
            return MembersWithAttributeOfType<T>(includeInherited).OfType<IPropertySymbol>();
        }

        public IEnumerable<IMethodSymbol> MethodsWithAttributeOfType<T>(bool includeInherited = false)
        {
            return MembersWithAttributeOfType<T>(includeInherited).OfType<IMethodSymbol>();
        }
    }
}
