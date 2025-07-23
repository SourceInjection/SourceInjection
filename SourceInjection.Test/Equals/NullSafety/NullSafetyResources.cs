using System.Diagnostics.CodeAnalysis;

#pragma warning disable

namespace SourceInjection.Test.Equals.NullSafety
{
    using NullSafety = SourceInjection.NullSafety;

    public static class NullSafetyResources
    {
        public static readonly Type[] MustBeNullSafe =
        {
            typeof(ClassWithNullableProperty),
            typeof(ClassWithProperty_NullableDisabled),
            typeof(ClassWithProperty_NullSafetyOn),
            typeof(ClassWithProperty_ThatHasMaybeNullAttribute),
        };

        public static readonly Type[] MustNotBeNullSafe =
        {
            typeof(ClassWithProperty),
            typeof(ClassWithProperty_NullableDisabled_NullSafetyOff),
            typeof(ClassWithProperty_ThatHasNotNullAttribute_NullableDisabled),
        };
    }

    [AutoEquals]
    public partial class DataClass
    {
        public int Value { get; set; }
    }

    [AutoEquals]
    public partial class ClassWithNullableProperty
    {
        public DataClass? Property { get; set; }
    }

    [AutoEquals]
    public partial class ClassWithProperty
    {
        public DataClass Property { get; set; } = null!;
    }


    [AutoEquals(nullSafety: NullSafety.On)]
    public partial class ClassWithProperty_NullSafetyOn
    {
        public DataClass Property { get; set; } = null!;
    }

#nullable disable
    [AutoEquals]
    public partial class ClassWithProperty_NullableDisabled
    {
        public DataClass Property { get; set; }
    }
#nullable restore

#nullable disable
    [AutoEquals(nullSafety: NullSafety.Off)]
    public partial class ClassWithProperty_NullableDisabled_NullSafetyOff
    {
        public DataClass Property { get; set; }
    }
#nullable restore


#nullable disable
    [AutoEquals]
    public partial class ClassWithProperty_ThatHasNotNullAttribute_NullableDisabled
    {
        [NotNull]
        public DataClass Property { get; set; }
    }
#nullable restore

    [AutoEquals]
    public partial class ClassWithProperty_ThatHasMaybeNullAttribute
    {
        [MaybeNull]
        public DataClass Property { get; set; }
    }
}

#pragma warning restore