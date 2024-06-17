
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

    public class ConstraintClause(ConstraintKind ckind, string? value = null)
    {
        public ConstraintKind Kind => ckind;

        public string? Value => value;

        public bool IsKind(ConstraintKind kind) => Kind == kind;
    }
}
