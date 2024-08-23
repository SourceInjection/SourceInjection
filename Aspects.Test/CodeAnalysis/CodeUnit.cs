using Aspects.SourceGenerators.Base;
using CodeUnits.CSharp;

namespace Aspects.Test.CodeAnalysis
{
    internal class CodeUnit
    {
        private static readonly string _projectDir = new DirectoryInfo(Environment.CurrentDirectory)
            .Parent?.Parent?.Parent?.FullName ?? string.Empty;

        private static readonly string _generatedDir = $"{_projectDir}\\obj\\"
#if DEBUG 
                + "Debug"
#elif RELEASE
                + "Release"
#endif
            + $"\\net8.0\\generated\\{nameof(Aspects)}";


        public static ICodeUnit FromGeneratedCode<TGenerator, TType>() 
            where TGenerator : TypeSourceGeneratorBase, new()
        {
            var filePath = $"{_generatedDir}\\{typeof(TGenerator).FullName}\\" +
                $"{typeof(TType).FullName}-{new TGenerator().Name}.g.cs";
            
            
            return CodeUnits.CSharp.CodeUnit.FromStream(File.OpenRead(filePath));
        }
    }
}
