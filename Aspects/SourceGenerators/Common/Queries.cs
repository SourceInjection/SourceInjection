
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class Queries
    {
        public static IEnumerable<IFieldSymbol> GetFieldsWithAttribute(
            GeneratorSyntaxContext context, TypeDeclarationSyntax node, string attributeFullName)
        {
            return node.ChildNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(field => field.AttributeLists.Any())
                .SelectMany(field => field.Declaration.Variables.Select(v => context.SemanticModel.GetDeclaredSymbol(v) as IFieldSymbol))
                .Where(field => field != null && field.GetAttributes().Any(fs => fs.AttributeClass.ToDisplayString() == attributeFullName));
        }

        public static string TypeDefinition(TypeDeclarationSyntax node)
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
    }
}
