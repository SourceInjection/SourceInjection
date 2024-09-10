namespace Aspects.Attributes.Interfaces
{
    public interface IEqualityComparerAttribute : IEqualityComparisonConfigAttribute { }

    public interface IEqualityComparisonConfigAttribute
    {
        string EqualityComparer { get; }

        NullSafety NullSafety { get; }
    }
}
