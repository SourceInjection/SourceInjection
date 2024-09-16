using Aspects.SourceGeneration.Base;
using CompileUnits.CSharp;

namespace Aspects.Test.CodeAnalysis
{
    internal static class CompileUnit
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


        public static ICompileUnit FromGeneratedCode<TGenerator, TType>() 
            where TGenerator : TypeSourceGeneratorBase, new()
        {
            return FromGeneratedCode<TGenerator>(typeof(TType));
        }

        public static ICompileUnit FromGeneratedCode<TGenerator>(Type type)
            where TGenerator : TypeSourceGeneratorBase, new()
        {
            var filePath = $"{_generatedDir}\\{typeof(TGenerator).FullName}\\" +
                $"{type.FullName!.Replace('.', '\\')}-{new TGenerator().Name}.g.cs";

            var fs = File.OpenRead(filePath);

            return CompileUnits.CSharp.CompileUnit.FromStream(fs);
        }
    }
}
