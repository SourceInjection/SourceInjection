namespace Aspects.Attributes.Interfaces
{
    public interface IEqualityComparerAttribute
    {
        string EqualityComparer { get; }

        NullSafety NullSafety { get; }
    }
}
