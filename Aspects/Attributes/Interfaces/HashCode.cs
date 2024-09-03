namespace Aspects.Attributes.Interfaces
{
    public interface IAutoHashCodeAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        BaseCall BaseCall { get; }
    }

    public interface IHashCodeAttribute 
    {
        string EqualityComparer { get; }
    }

    public interface IHashCodeExcludeAttribute { }
}
