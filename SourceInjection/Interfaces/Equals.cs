namespace SourceInjection.Interfaces
{
    public interface IAutoEqualsAttribute 
    {
        /// <summary>
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/>.<br/>
        /// </summary>
        DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </summary>
        BaseCall BaseCall { get; }

        /// <summary>
        /// Determines if null safe operations are used.
        /// </summary>
        NullSafety NullSafety { get; }
    }

    public interface IEqualsAttribute 
    {
        NullSafety NullSafety { get; }
    }

    public interface IEqualsExcludeAttribute { }
}
