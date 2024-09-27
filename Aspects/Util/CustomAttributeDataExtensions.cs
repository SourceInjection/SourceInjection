using Aspects.SourceGeneration.Common;
using System.Reflection;

namespace Aspects.CodeAnalysis
{
    internal static class CustomAttributeDataExtensions
    {
        public static bool IsNotNullAttribute(this CustomAttributeData attr)
        {
            return attr.AttributeType.FullName == NameOf.NotNullAttribute;
        }

        public static bool IsMaybeNullAttribute(this CustomAttributeData attr)
        {
            return attr.AttributeType.FullName == NameOf.MaybeNullAttribute;
        }

        public static bool IsAllowNullAttribute(this CustomAttributeData attr)
        {
            return attr.AttributeType.FullName == NameOf.AllowNullAttribute;
        }

        public static bool IsDisallowNullAttribute(this CustomAttributeData attr)
        {
            return attr.AttributeType.FullName == NameOf.DisallowNullAttribute;
        }
    }
}
