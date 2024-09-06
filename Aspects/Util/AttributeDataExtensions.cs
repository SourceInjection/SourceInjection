using Aspects.Common.Paths;
using Microsoft.CodeAnalysis;

namespace Aspects.Util
{
    internal static class AttributeDataExtensions
    {
        public static bool IsNotNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.NotNullAttribute;
        }

        public static bool IsMayBeNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.MaybeNullAttribute;
        }
    }
}
