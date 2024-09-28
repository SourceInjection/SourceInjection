using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.Equals(object)"/> generation.<br/>
    /// All members with <see cref="EqualsExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="EqualsAttribute"/>.<br/>
    /// On default properties and fields are automaticly merged by scanning the getter code in properties.<br/>
    /// Linked fields can only be detected when the property fullfilles the following grammar:<br/>
    /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAttribute : Attribute, IAutoEqualsAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="AutoEqualsAttribute"/>.
        /// </summary>
        /// <param name="dataMemberKind">
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.<br/>
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        /// <param name="baseCall">
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </param>
        /// <param name="nullSafety">
        /// Determines if the equalization is generated null safe.
        /// </param>
        public AutoEqualsAttribute(
            DataMemberKind dataMemberKind = DataMemberKind.DataMember,
            BaseCall baseCall = BaseCall.Auto,
            NullSafety nullSafety = NullSafety.Auto)
        {
            DataMemberKind = dataMemberKind;
            BaseCall = baseCall;
            NullSafety = nullSafety;
        }

        /// <summary>
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/>.<br/>
        /// </summary>
        public DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </summary>
        public BaseCall BaseCall { get; }

        /// <summary>
        /// Determines if null safe operations are used.
        /// </summary>
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
        /// <summary>
        /// Creates an instance of <see cref="EqualsAttribute"/>.
        /// </summary>
        /// <param name="nullSafety">Determines if null safe equalizations are used.</param>
        /// <param name="equalityComparer">Determines the comparer which is then used to compare for equalization.</param>
        public EqualsAttribute(Type equalityComparer = null, NullSafety nullSafety = NullSafety.Auto)
            : this(equalityComparer?.FullName, nullSafety) { }

        private EqualsAttribute(string equalityComparer = null, NullSafety nullSafety = NullSafety.Auto)
        {
            NullSafety = nullSafety;
            EqualityComparer = equalityComparer;
        }

        /// <summary>
        /// Determines if null safe operations are used.
        /// </summary>
        public NullSafety NullSafety { get; }

        /// <summary>
        /// The equality comparer which is used to compare the properties.
        /// </summary>
        public string EqualityComparer { get; }
    }

    /// <summary>
    /// Must be used in combination with <see cref="AutoEqualsAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsExcludeAttribute : Attribute, IEqualsExcludeAttribute { }
}
