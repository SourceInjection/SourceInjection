using Aspects.SourceGenerators.SyntaxReceivers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators.Base
{
    public abstract class TypeSourceGeneratorBase : ISourceGenerator
    {
        protected private abstract string Name { get; }

        protected private abstract TypeSyntaxReceiver SyntaxReceiver { get; }


        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => SyntaxReceiver);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxContextReceiver == SyntaxReceiver)
            {
                foreach (var typeInfo in SyntaxReceiver.IdentifiedTypes)
                {
                    var src = GeneratePartialType(typeInfo);
                    context.AddSource($"{typeInfo.Symbol.Name}_{Name}.g.cs", SourceText.From(src, Encoding.UTF8));
                }
            }
        }

        protected private abstract string GeneratePartialType(TypeInfo typeInfo);
    }
}
