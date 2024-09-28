using Microsoft.CodeAnalysis;

namespace SourceInjection.CodeAnalysis
{
    public static class FieldSymbolExtensions
    {
        public static bool IsInstanceMember(this IFieldSymbol field)
        {
            return !field.IsConst && !field.IsStatic;
        }
    }
}
