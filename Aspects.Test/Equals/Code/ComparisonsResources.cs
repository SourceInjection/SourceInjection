using Aspects.Attributes;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS0659, S3249, S2094, S3887, CA2231

namespace Aspects.Test.Equals.Code
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

    [AutoEquals]
    public partial class ReferenceType_WithCollectionsEquatableBySequenceEqual
    {
        public static readonly string[] Properties
            = typeof(ReferenceType_WithCollectionsEquatableBySequenceEqual).GetProperties().Select(p => p.Name).ToArray();

        public int[] IntArray { get; } = null!;

        public IEnumerable<int> IntEnumerable { get; } = null!;

        public IList<int> IntIList { get; } = null!;

        public IReadOnlyDictionary<int, string> ReadOnlyDicionary { get; } = null!;

        public ImmutableArray<int>.Builder Builder { get; } = null!;
    }

    [AutoEquals]
    public partial class ReferenceType_WithCollectionsNotEquatableBySequenceEqual
    {
        public static readonly string[] Properties
            = typeof(ReferenceType_WithCollectionsNotEquatableBySequenceEqual).GetProperties().Select(p => p.Name).ToArray();

        public IEnumerable Enumerable { get; } = null!;

        public IList<IEnumerable> CollectionList { get; } = null!;
    }

    [AutoEquals]
    public partial class ReferenceType_WithMultiDimensionalArrays
    {
        public static readonly string[] Properties
            = typeof(ReferenceType_WithMultiDimensionalArrays).GetProperties().Select(p => p.Name).ToArray();

        public int[,] IntDim2 { get; } = null!;

        public object[,,] ObjectDim3 { get; } = null!;
    }

    [AutoEquals]
    public partial class ReferenceType_WithNullableCollectionEquatableBySequenceEqual
    {
        public IEnumerable<string>? Property { get; }
    }

    public partial class ReferenceType_WithMemberThatHasCustomComparer
    {
        private class IntComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode([DisallowNull] int obj)
            {
                throw new NotImplementedException();
            }
        }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }

    [AutoEquals]
    public partial class ReferenceType_WithDefaultSettings_WithQueryProperty
    {
        public int Property => 3;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ReferenceType_WithDataMemberKindProperty_WithQueryProperty
    {
        public int Property => 3;
    }


    public partial class ReferenceType_WithQueryProperty_WithEqualsInclude
    {
        [Equals]
        public int Property => 3;
    }

    [AutoEquals]
    public partial class ReferenceType_WithConstantField
    {
        public const int _int = 3;
    }

    public partial class ReferenceType_WithConstantField_WithEqualsInclude
    {
        [Equals]
        public const int _int = 3;
    }

    [AutoEquals]
    public partial class ReferenceType_WithStaticField
    {
        static int s_int = 3;
    }

    public partial class ReferenceType_WithStaticField_WithEqualsInclude
    {
        [Equals]
        static int s_int = 3;
    }
}

#pragma warning restore CS0659, S3249, S2094, S3887, CA2231
