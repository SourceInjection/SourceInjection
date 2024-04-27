using System;

namespace Aspects.Attributes
{
    public enum DataMemberKind 
    {
        ProjectConfig = 0,
        Field = 1,
        Property = 2,      
        DataMember = Field | Property
    }

    public enum Accessibility
    {
        Private = 1,
        ProtectedPrivate = 2,
        Protected = 4,
        Internal = 8,
        ProtectedInternal = 16,
        Public = 32,
    }

    public static class AccessibilityExtensions
    {
        internal static string ToDisplayString(this Accessibility accessability)
        {
            switch (accessability)
            {
                case Accessibility.Private: return "private";
                case Accessibility.Protected: return "protected";
                case Accessibility.Internal: return "internal";
                case Accessibility.Public: return "public";
                case Accessibility.ProtectedInternal: return "protected internal";
                case Accessibility.ProtectedPrivate: return "protected private";
                default: throw new NotImplementedException();  
            }
        }
    }
}
