using Aspects.Attributes;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable

namespace Aspects.Test.Equals.NullSafety
{
    using NullSafety = Attributes.NullSafety;

    public class ClassEmpty { }

    [AutoEquals]
    public partial class ClassWithNullableProperty
    {
        public ClassEmpty? Property { get; set; }
    }

    [AutoEquals]
    public partial class ClassWithProperty
    {
        public ClassEmpty Property { get; set; } = null!;
    }


    [AutoEquals(nullSafety: NullSafety.On)]
    public partial class ClassWithProperty_NullSafetyOn
    {
        public ClassEmpty Property { get; set; } = null!;
    }

#nullable disable
    [AutoEquals]
    public partial class ClassWithProperty_NullableDisabled
    {
        public ClassEmpty Property { get; set; }
    }
#nullable restore

#nullable disable
    [AutoEquals(nullSafety: NullSafety.Off)]
    public partial class ClassWithProperty_NullableDisabled_NullSafetyOff
    {
        public ClassEmpty Property { get; set; }
    }
#nullable restore


#nullable disable
    [AutoEquals]
    public partial class ClassWithProperty_ThatHasNotNullAttribute_NullableDisabled
    {
        [NotNull]
        public ClassEmpty Property { get; set; }
    }
#nullable restore

    [AutoEquals]
    public partial class ClassWithProperty_ThatHasMaybeNullAttribute
    {
        [MaybeNull]
        public ClassEmpty Property { get; set; }
    }
}

#pragma warning restore