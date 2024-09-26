using Aspects.SourceGeneration.Common;
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

        public static bool IsAllowNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.AllowNullAttribute;
        }

        public static bool IsDisallowNullAttribute(this AttributeData attData)
        {
            return attData.AttributeClass?.ToDisplayString() == NameOf.DisallowNullAttribute;
        }
    }
}
