using Aspects.Attributes;
using System.Collections;
using System.Collections.Immutable;

#pragma warning disable

namespace Aspects.Test.Equals.Comparisons
{
    using NullSafety = Attributes.NullSafety;

    [AutoEquals]
    public partial class ClassEmpty { }

    [AutoEquals]
    public readonly partial struct NonReferenceTypeEmpty { }

    public record RecordEmpty { }

    public enum En { A, B }

    [AutoEquals]
    public partial class ClassWithProperties_ThatArePropertiesWithDefaultEqualOperators
    {
        public static readonly string[] Properties
            = typeof(ClassWithProperties_ThatArePropertiesWithDefaultEqualOperators).GetProperties().Select(p => p.Name).ToArray();

        public int? NullableInt { get; }

        public int Int { get; }

        public RecordEmpty RecordType { get; } = null!;

        public RecordEmpty? NullableRecordType { get; } = null!;

        public string? NullableString { get; }

        public string String { get; } = null!;

        public En Enum { get; }
    }

    [AutoEquals]
    public partial class ClassWithCollections_ThatAreEquatableBySequenceEqual
    {
        public static readonly string[] Properties
            = typeof(ClassWithCollections_ThatAreEquatableBySequenceEqual).GetProperties().Select(p => p.Name).ToArray();

        public int[] IntArray { get; } = null!;

        public IEnumerable<int> IntEnumerable { get; } = null!;

        public IList<int> IntIList { get; } = null!;

        public IReadOnlyDictionary<int, string> ReadOnlyDicionary { get; } = null!;

        public ImmutableArray<int>.Builder Builder { get; } = null!;
    }

    [AutoEquals]
    public partial class ClassWithCollections_ThatAreNotEquatableBySequenceEqual
    {
        public static readonly string[] Properties
            = typeof(ClassWithCollections_ThatAreNotEquatableBySequenceEqual).GetProperties().Select(p => p.Name).ToArray();

        public IEnumerable Enumerable { get; } = null!;

        public IList<IEnumerable> CollectionList { get; } = null!;
    }

    [AutoEquals]
    public partial class ClassWithMultiDimensionalArrays
    {
        public static readonly string[] Properties
            = typeof(ClassWithMultiDimensionalArrays).GetProperties().Select(p => p.Name).ToArray();

        public int[,] IntDim2 { get; } = null!;

        public object[,,] ObjectDim3 { get; } = null!;
    }

    [AutoEquals]
    public partial class ClassWithNullableCollection_ThatIsEquatableBySequenceEqual
    {
        public IEnumerable<string>? Property { get; }
    }
}

#pragma warning restore
