namespace SourceInjection.Interfaces
{
    public interface IEqualityComparerAttribute
    {
        /// <summary>
        /// The <see cref="IEqualityComparer"/> which is used to compare or evaluating the hash code.
        /// </summary>
        string EqualityComparer { get; }

        /// <summary>
        /// Determines if null safe operations are used.
        /// </summary>
        NullSafety NullSafety { get; }
    }
}
