﻿using Aspects.SourceGenerators.Common.LoadingBehaviors;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal class TypeInfo
    {
        private readonly Lazy<bool> _hasPartialModifier;
        private readonly Lazy<string> _declaration;
        private readonly Lazy<string> _nameWithGenericParameters;
        private readonly Lazy<string> _fullNameWithGenericParameters;
        private readonly Lazy<List<INamedTypeSymbol>> _inheritanceFromBottomToTop;
        private readonly Lazy<List<PropertyInfo>> _localPropertyInfos;
        private readonly Lazy<bool> _hasNullableEnabled;

        public TypeInfo(GeneratorSyntaxContext context, TypeDeclarationSyntax syntaxNode, INamedTypeSymbol typeSymbol)
        {
            SyntaxNode = syntaxNode;
            Symbol = typeSymbol;

            _hasNullableEnabled = new Lazy<bool>(() => context.SemanticModel.GetNullableContext(0).AnnotationsEnabled() 
                && !syntaxNode.HasNullableDisabledDirective());
            _hasPartialModifier = new Lazy<bool>(() => syntaxNode.HasPartialModifier());
            _declaration = new Lazy<string>(() => syntaxNode.Declaration());
            _nameWithGenericParameters = new Lazy<string>(() => syntaxNode.NameWithGenericParameters());
            _fullNameWithGenericParameters = new Lazy<string>(() => syntaxNode.FullNameWithGenericParameters());
            _inheritanceFromBottomToTop = new Lazy<List<INamedTypeSymbol>>(() => typeSymbol.InheritanceFromBottomToTop().ToList());
            _localPropertyInfos = new Lazy<List<PropertyInfo>>(() => LoadProperties(context));
        }

        public TypeDeclarationSyntax SyntaxNode { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool HasPartialModifier => _hasPartialModifier.Value;

        public string Declaration => _declaration.Value;

        public string NameWithGenericParameters => _nameWithGenericParameters.Value;

        public string FullNameWithGenericParameters => _fullNameWithGenericParameters.Value;

        public bool HasNullableEnabled => _hasNullableEnabled.Value;

        public IEnumerable<PropertyInfo> LocalPropertyInfos => _localPropertyInfos.Value;

        public IEnumerable<INamedTypeSymbol> InheritanceFromBottomToTop(bool includeSelf = false)
        {
            if (includeSelf)
                return _inheritanceFromBottomToTop.Value;
            return _inheritanceFromBottomToTop.Value.Take(_inheritanceFromBottomToTop.Value.Count - 1);
        }

        public IEnumerable<ISymbol> Members(bool includeInherited = false)
        {
            if (!includeInherited)
                return Symbol.GetMembers();

            return InheritanceFromBottomToTop(true).SelectMany(cl => cl.GetMembers());
        }

        private List<PropertyInfo> LoadProperties(GeneratorSyntaxContext context)
        {
            var localProperties = new HashSet<IPropertySymbol>(
                Symbol.GetMembers().OfType<IPropertySymbol>(), SymbolEqualityComparer.Default);

            return SyntaxNode.DescendantNodes()
                .OfType<PropertyDeclarationSyntax>()
                .Select(pds => new { PropertyDeclarationSyntax = pds, Symbol = context.SemanticModel.GetDeclaredSymbol(pds) })
                .Where(tuple => tuple.Symbol is IPropertySymbol && localProperties.Contains(tuple.Symbol, SymbolEqualityComparer.Default))
                .Select(tuple => new PropertyInfo(tuple.PropertyDeclarationSyntax, (IPropertySymbol)tuple.Symbol))
                .ToList();
        }
    }
}
