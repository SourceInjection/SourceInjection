using System;

namespace Aspects.Attributes.Interfaces
{
    public interface IAutoEqualsAttribute 
    {
        DataMemberKind DataMemberKind { get; }

        BaseCall BaseCall { get; }

        NullSafety NullSafety { get; }
    }

    public interface IEqualsAttribute 
    { 
        NullSafety NullSafety { get; }

        string EqualityComparer { get; }
    }

    public interface IEqualsExcludeAttribute { }
}
