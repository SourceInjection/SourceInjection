using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// Combines attributes <see cref="AutoEqualsAttribute"/> and <see cref="AutoHashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAndHashCodeAttribute : Attribute, IAutoEqualsAttribute, IAutoHashCodeAttribute
    {
        public AutoEqualsAndHashCodeAttribute(
            DataMemberKind dataMemberKind = DataMemberKind.DataMember,
            BaseCall baseCall = BaseCall.Auto,
            NullSafety nullSafety = NullSafety.Auto,
            bool storeHashCode = false)
        {
            DataMemberKind = dataMemberKind;
            BaseCall = baseCall;
            NullSafety = nullSafety;
            StoreHashCode = storeHashCode;
        }

        public DataMemberKind DataMemberKind { get; }

        public BaseCall BaseCall { get; }

        public NullSafety NullSafety { get; }

        public bool StoreHashCode { get; }
    }

    /// <summary>
    /// Combines attributes <see cref="EqualsAttribute"/> and <see cref="HashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeAttribute : Attribute, IEqualsAttribute, IHashCodeAttribute
    {
        public EqualsAndHashCodeAttribute(NullSafety nullSafety = NullSafety.Auto)
        {
            NullSafety = nullSafety;
        }

        public NullSafety NullSafety { get; }
    }

    /// <summary>
    /// Combines attributes <see cref="EqualsExcludeAttribute"/> and <see cref="HashCodeExcludeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute, IEqualsExcludeAttribute, IHashCodeExcludeAttribute { }
}
