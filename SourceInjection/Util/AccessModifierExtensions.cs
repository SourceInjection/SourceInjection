using Microsoft.CodeAnalysis;

namespace SourceInjection.Util
{
    internal static class AccessibilityExtensions
    {
        public static AccessModifier ToAccessModifier(this Accessibility accessibility)
        {
            if (accessibility == Accessibility.Public)
                return AccessModifier.Public;

            if (accessibility == Accessibility.Private)
                return AccessModifier.Private;

            if (accessibility == Accessibility.Internal)
                return AccessModifier.Internal;

            if (accessibility == Accessibility.Protected)
                return AccessModifier.Protected;

            if (accessibility == Accessibility.ProtectedOrInternal)
                return AccessModifier.ProtectedInternal;

            if (accessibility == Accessibility.ProtectedAndInternal)
                return AccessModifier.ProtectedPrivate;

            return AccessModifier.None;
        }
    }
}
