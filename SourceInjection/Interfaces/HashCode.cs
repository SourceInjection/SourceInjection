namespace SourceInjection.Interfaces
{
    public interface IAutoHashCodeAttribute 
    {
        /// <summary>
        /// Defines which data members are used to generate <see cref="object.ToString"/>.<br/>
        /// </summary>
        DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.GetHashCode() is called.
        /// </summary>
        BaseCall BaseCall { get; }

        /// <summary>
        /// Determines if the hash code is stored in a local field.
        /// </summary>
        bool StoreHashCode { get; }
    }

    public interface IHashCodeAttribute { }

    public interface IHashCodeExcludeAttribute { }
}
