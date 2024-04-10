using System;

namespace Aspects.Attributes
{
    public enum SetterVisibility
    {
        Public,
        Internal,
        Protected,
        Private,
        ProtectedInternal,
        ProtectedPrivate,
    }

    internal static class SetterVisibilityExtensions
    {
        public static string ToDisplayString(this SetterVisibility visibility)
        {
            switch(visibility)
            {
                case SetterVisibility.Public: return "public";
                case SetterVisibility.Internal: return "internal";
                case SetterVisibility.Protected: return "protected";
                case SetterVisibility.Private: return "private";
                case SetterVisibility.ProtectedInternal: return "protected internal";
                case SetterVisibility.ProtectedPrivate: return "protected private";
                default: throw new NotImplementedException();
            }
        }
    }
}
