namespace Aspects.Attributes.Interfaces
{
    public interface IAutoHashCodeAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        BaseCall BaseCall { get; }

        bool StoreHashCode { get; }
    }

    public interface IHashCodeAttribute : IEqualityComparerAttribute { }

    public interface IHashCodeExcludeAttribute { }
}
