using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.GetHashCode"/> generation.<br/>
    /// All members with <see cref="HashCodeExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="HashCodeAttribute"/>.<br/>
    /// On default properties and fields are automaticly merged by scanning the getter code in properties.<br/>
    /// Linked fields can only be detected when the property fullfilles the following grammar:<br/>
    /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoHashCodeAttribute : Attribute, IAutoHashCodeAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="AutoHashCodeAttribute"/>.
        /// </summary>
        /// <param name="dataMemberKind">
        /// Defines which data members are used to generate <see cref="object.GetHashCode()"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.<br/>
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        /// <param name="baseCall">
        /// Determines if <see langword="base"/>.GetHashCode() is called.
        /// </param>
        public AutoHashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember, BaseCall baseCall = BaseCall.Auto, bool storeHashCode = false)
        {
            BaseCall = baseCall;
            DataMemberKind = dataMemberKind;
            StoreHashCode = storeHashCode;
        }

        /// <summary>
        /// Defines which data members are used to generate <see cref="object.ToString"/>.<br/>
        /// </summary>
        public DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.GetHashCode() is called.
        /// </summary>
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
    {
        public HashCodeAttribute()
            : this(default(string), NullSafety.Auto)
        { }

        public HashCodeAttribute(Type equalityComparer, NullSafety nullSafety = NullSafety.Auto)
            : this(equalityComparer?.FullName, nullSafety)
        { }

        private HashCodeAttribute(string equalityComparer, NullSafety nullSafety = NullSafety.Auto)
        {
            EqualityComparer = equalityComparer;
            NullSafety = nullSafety;
        }

        public string EqualityComparer { get; }

        public NullSafety NullSafety { get; }
    }

    /// <summary>
    /// Must be used in combination with <see cref="AutoHashCodeAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class HashCodeExcludeAttribute : Attribute, IHashCodeExcludeAttribute { }
}
