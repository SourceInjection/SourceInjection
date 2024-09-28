namespace SourceInjection.Interfaces
{
    public interface IAutoToStringAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        Accessibility Accessibility { get; }
    }

    public interface IToStringAttribute 
    {
        string Label { get; }

        string Format { get; }
    }

    public interface IToStringExcludeAttribute { }
}
