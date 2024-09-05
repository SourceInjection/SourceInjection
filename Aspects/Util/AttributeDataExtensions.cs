using Aspects.SourceGenerators.Common;
using Microsoft.CodeAnalysis;

namespace Aspects.Util
{
    internal static class AttributeDataExtensions
    {
        /// <summary>
        /// Checks if the given <see cref="AttributeData"/> is a System.Diagnostics.CodeAnalysis.NotNullAttribute.
        /// </summary>
        /// <param name="attData"></param>
        /// <returns><see langword="true"/> if the given <see cref="AttributeData"/> is a System.Diagnostics.CodeAnalysis.NotNullAttribute else <see langword="false"/>.</returns>
        public static bool IsNotNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.NotNullAttribute;
        }

        /// <summary>
        /// Checks if the given <see cref="AttributeData"/> is a System.Diagnostics.CodeAnalysis.MaybeNullAttribute.
        /// </summary>
        /// <param name="attData"></param>
        /// <returns><see langword="true"/> if the given <see cref="AttributeData"/> is a System.Diagnostics.CodeAnalysis.MaybeNullAttribute else <see langword="false"/>.</returns>
        public static bool IsMayBeNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.MaybeNullAttribute;
        }
    }
}
