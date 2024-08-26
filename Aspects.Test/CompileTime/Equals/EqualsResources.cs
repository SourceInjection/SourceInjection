using Aspects.Attributes;

namespace Aspects.Test.CompileTime.Equals
{
    [AutoEquals]
    public partial class ReferenceType { }

    [AutoEquals]
    public readonly partial struct NonReferenceType { }

    [AutoEquals]
    public partial class ReferenceTypeWithNullableProperty
    {
        public ReferenceType? Property { get; }
    }

    [AutoEquals]
    public partial class ReferenceTypeWithNonNullableProperty
    {
        public ReferenceType Property { get; } = null!;
    }

#nullable disable
    [AutoEquals]
    public partial class ReferenceTypeWithPropertyNullableDisabled
    {
        public ReferenceType Property { get; }
    }
#nullable restore
}
