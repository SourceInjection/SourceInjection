namespace Aspects.Interfaces
{
    public interface IAutoEqualsAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        BaseCall BaseCall { get; }

        NullSafety NullSafety { get; }
    }

    public interface IEqualsAttribute : IEqualityComparisonConfigAttribute { }

    public interface IEqualsExcludeAttribute { }
}
