namespace Aspects.Attributes.Interfaces
{
    public interface IAutoToStringAttribute 
    {
        DataMemberKind DataMemberKind { get; }
    }

    public interface IToStringAttribute : IUseOnPropertyGenerationAttribute { }

    public interface IToStringExcludeAttribute { }
}
