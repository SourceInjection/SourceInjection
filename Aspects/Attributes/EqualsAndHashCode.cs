using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    /// <summary>
    /// Combines attributes <see cref="AutoEqualsAttribute"/> and <see cref="AutoHashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAndHashCodeAttribute : Attribute, IAutoEqualsAttribute, IAutoHashCodeAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="AutoEqualsAndHashCodeAttribute"/>.
        /// </summary>
        /// <param name="dataMemberKind">
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        /// <param name="baseCall">
        /// Determines if <see langword="base"/>.Equals() and <see langword="base"/>.GetHashCode() is called.
        /// </param>
        /// <param name="nullSafety">
        /// Determines if the equals check is generated null safe.
        /// </param>
        public AutoEqualsAndHashCodeAttribute(
            DataMemberKind dataMemberKind = DataMemberKind.DataMember,
            BaseCall baseCall = BaseCall.Auto,
            NullSafety nullSafety = NullSafety.Auto)
        { 
            DataMemberKind = dataMemberKind;
            BaseCall = baseCall;
            NullSafety = nullSafety;
        }

        /// <summary>
        /// Determines the <see cref="DataMemberKind"/> of the <see cref="object.GetHashCode()"/> method generation.
        /// </summary>
        public DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </summary>
        public BaseCall BaseCall { get; }

        /// <summary>
        /// Determines if the equalization is generated nullsafe.
        /// </summary>
        public NullSafety NullSafety { get; }
    }

    /// <summary>
    /// Combines attributes <see cref="EqualsAttribute"/> and <see cref="HashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeAttribute : Attribute, IEqualsAttribute, IHashCodeAttribute 
    {
        /// <summary>
        /// Creates an instance of <see cref="EqualsAndHashCodeAttribute"/>.
        /// </summary>
        /// <param name="equalsNullSafety">Determines if the equalization is generated null safe.</param>
        /// <param name="equalsEqualityComparer">Determines the comparer which is then used to compare for equalization.</param>
        public EqualsAndHashCodeAttribute(NullSafety equalsNullSafety = NullSafety.Auto, Type equalsEqualityComparer = null)
            : this(equalsNullSafety, equalsEqualityComparer?.FullName)
        { }

        private EqualsAndHashCodeAttribute(NullSafety nullSafety = NullSafety.Auto, string equalityComparer = null) 
        {
            NullSafety = nullSafety;
            EqualityComparer = equalityComparer;
        }

        /// <summary>
        /// The equality comparer which is evaluate the hash code and perform equalization of the data member.
        /// </summary>
        public string EqualityComparer { get; }


        /// <summary>
        /// Determines if the equalization is generated nullsafe.
        /// </summary>
        public NullSafety NullSafety { get; }

    }

    /// <summary>
    /// Combines attributes <see cref="EqualsExcludeAttribute"/> and <see cref="HashCodeExcludeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute, IEqualsExcludeAttribute, IHashCodeExcludeAttribute { }
}
