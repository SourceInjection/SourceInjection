namespace SourceInjection.Interfaces
{
    public interface IAutoHashCodeAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        BaseCall BaseCall { get; }

        bool StoreHashCode { get; }
    }

    public interface IHashCodeAttribute : IEqualityComparisonConfigAttribute { }

    public interface IHashCodeExcludeAttribute { }
}
