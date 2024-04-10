
using Microsoft.CodeAnalysis;
using System.Collections.Generic;

namespace Aspects.Helpers
{
    internal static class Dependencies
    {
        public static IEnumerable<string> TypeDeclarationDependencies(INamedTypeSymbol symbol)
        {
            yield break;
        }
    }
}
