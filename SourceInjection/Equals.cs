using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.Equals(object)"/> generation.<br/>
    /// All members with <see cref="EqualsExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="EqualsAttribute"/>.<br/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAttribute : Attribute, IAutoEqualsAttribute
    {
        public AutoEqualsAttribute(
            DataMemberKind dataMemberKind = DataMemberKind.DataMember,
            BaseCall baseCall = BaseCall.Auto,
            NullSafety nullSafety = NullSafety.Auto)
        {
            DataMemberKind = dataMemberKind;
            BaseCall = baseCall;
            NullSafety = nullSafety;
        }

        public DataMemberKind DataMemberKind { get; }

        public BaseCall BaseCall { get; }

        public NullSafety NullSafety { get; }
    }

    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.Equals(object)"/> generation.<br/>
    /// All members with this attribute will be used to generate <see cref="object.Equals(object)"/>.<br/>
    /// If used in combination with <see cref="AutoEqualsAttribute"/> this attribute includes members that are normally excluded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAttribute : Attribute, IEqualsAttribute
    {
        public EqualsAttribute(NullSafety nullSafety = NullSafety.Auto)
        {
            NullSafety = nullSafety;
        }

        public NullSafety NullSafety { get; }
    }

    /// <summary>
    /// Must be used in combination with <see cref="AutoEqualsAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsExcludeAttribute : Attribute, IEqualsExcludeAttribute { }
}
