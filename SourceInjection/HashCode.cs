using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.GetHashCode"/> generation.<br/>
    /// All members with <see cref="HashCodeExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="HashCodeAttribute"/>.<br/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoHashCodeAttribute : Attribute, IAutoHashCodeAttribute
    {
        public AutoHashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember, BaseCall baseCall = BaseCall.Auto, bool storeHashCode = false)
        {
            BaseCall = baseCall;
            DataMemberKind = dataMemberKind;
            StoreHashCode = storeHashCode;
        }

        public DataMemberKind DataMemberKind { get; }

        public BaseCall BaseCall { get; }

        public bool StoreHashCode { get; }
    }

    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.GetHashCode"/> generation.<br/>
    /// All members with this attribute will be used to generate <see cref="object.GetHashCode"/>.<br/>
    /// Also includes members that are normally excluded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class HashCodeAttribute : Attribute, IHashCodeAttribute
    { }

    /// <summary>
    /// Must be used in combination with <see cref="AutoHashCodeAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class HashCodeExcludeAttribute : Attribute, IHashCodeExcludeAttribute { }
}
