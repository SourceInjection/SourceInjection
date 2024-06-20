
namespace Aspects.Parsers.CSharp
{
    public enum ConstraintKind 
    { 
        Constructor, 
        OfType,
        Class,
        ClassNullable,
        Struct, 
        Unmanaged,
        NotNull, 
        Default 
    }

    public class ConstraintClause
    {
        public ConstraintClause(ConstraintKind kind, string? value = null)
        {
            Kind = kind;
            Value = value;
        }

        public ConstraintKind Kind { get; }

        public string? Value { get; }

        public bool IsKind(ConstraintKind kind) => Kind == kind;
    }
}
