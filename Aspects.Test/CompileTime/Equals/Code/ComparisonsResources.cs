using Aspects.Attributes;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS0659

namespace Aspects.Test.CompileTime.Equals.Comparisons
{
    public partial class ReferenceTypeEmpty_NoAttribute { }

    [AutoEquals]
    public partial class ReferenceTypeEmpty { }

    [AutoEquals]
    public readonly partial struct NonReferenceTypeEmpty { }

    [AutoEquals(baseCall: BaseCall.On)]
    public readonly partial struct NonReferenceTypeEmpty_WithBaseCallOn { }

    [AutoEquals]
    public partial class ReferenceTypeWithNullableProperty
    {
        public ReferenceTypeEmpty? Property { get; }
    }

    [AutoEquals]
    public partial class ReferenceTypeWithProperty
    {
        public ReferenceTypeEmpty Property { get; } = null!;
    }

    [AutoEquals(nullSafety: NullSafety.On)]
    public partial class ReferenceTypeWithProperty_NullSafetyOn
    {
        public ReferenceTypeEmpty Property { get; } = null!;
    }

#nullable disable
    [AutoEquals]
    public partial class ReferenceTypeWithProperty_NullableDisabled
    {
        public ReferenceTypeEmpty Property { get; }
    }
#nullable restore

#nullable disable
    [AutoEquals(nullSafety: NullSafety.Off)]
    public partial class ReferenceTypeWithProperty_NullableDisabled_NullSafetyOff
    {
        public ReferenceTypeEmpty Property { get; }
    }
#nullable restore


    [AutoEquals]
    public partial class ReferenceTypeEmptyWithBase_ThatHasAutoEquals : ReferenceTypeEmpty { }

    [AutoEquals(baseCall: BaseCall.Off)]
    public partial class ReferenceTypeEmptyWithBase_ThatHasAutoEquals_WithBaseCallOff : ReferenceTypeEmpty { }

    [AutoEquals(baseCall: BaseCall.On)]
    public partial class ReferenceTypeEmpty_WithBaseCallOn { }


    public partial class ReferenceTypeWithPropery_ThatHasEqualsAttribute
    {
        [Equals]
        public ReferenceTypeEmpty Property { get; } = null!;
    }

    public partial class ReferenceTypeWithEqualsOverride
    {
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }


    [AutoEquals]
    public partial class ReferenceTypeWithBase_ThatHasEqualsOnMember : ReferenceTypeWithPropery_ThatHasEqualsAttribute { }

    [AutoEquals]
    public partial class ReferenceTypeWithBase_ThatHasNoEqualsOverride : ReferenceTypeEmpty_NoAttribute { }

    [AutoEquals]
    public partial class ReferenceTypeWithBase_ThatHasEqualsOverride : ReferenceTypeWithEqualsOverride { }

#nullable disable
    [AutoEquals]
    public partial class ReferenceTypeWithProperty_ThatHasNotNullAttribute_NullableDisabled
    {
        [NotNull]
        public ReferenceTypeEmpty Property { get; }
    }
#nullable restore

    [AutoEquals]
    public partial class ReferenceTypeWithProperty_ThatHasMaybeNullAttribute
    {
        [MaybeNull]
        public ReferenceTypeEmpty Property { get; }
    }


    public record RecordType { }

    public enum En { A, B }

    [AutoEquals]
    public partial class ReferenceTypeWithProperties_ThatAllArePropertiesWithDefaultEqualOperators
    {
        public static readonly string[] Properties
            = typeof(ReferenceTypeWithProperties_ThatAllArePropertiesWithDefaultEqualOperators).GetProperties().Select(p => p.Name).ToArray();

        public int? NullableInt { get; }

        public int Int { get; }

        public RecordType RecordType { get; } = null!;

        public RecordType? NullableRecordType { get; } = null!;

        public string? NullableString { get; }

        public string String { get; } = null!;

        public En Enum { get; }
    }
}

#pragma warning restore CS0659
