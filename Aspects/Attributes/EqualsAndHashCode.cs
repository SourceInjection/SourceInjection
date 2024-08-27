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
        /// <param name="equalsDataMemberKind">
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        /// <param name="hashCodeDataMemberKind">
        /// Defines which data members are used to generate <see cref="object.GetHashCode()"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        /// <param name="equalsBaseCall">
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </param>
        /// <param name="hashCodeBaseCall">
        /// Determines if <see langword="base"/>.GetHashCode() is called.
        /// </param>
        /// <param name="equalsNullSafety">
        /// Determines if the equals check is generated null safe.
        /// </param>
        public AutoEqualsAndHashCodeAttribute(
            DataMemberKind equalsDataMemberKind = DataMemberKind.DataMember,
            BaseCall equalsBaseCall = BaseCall.Auto,
            NullSafety equalsNullSafety = NullSafety.Auto,
            DataMemberKind hashCodeDataMemberKind = DataMemberKind.DataMember,
            BaseCall hashCodeBaseCall = BaseCall.Auto)
        { 
            EqualsDataMemberKind = equalsDataMemberKind;
            EqualsBaseCall = equalsBaseCall;
            EqualsNullSafety = equalsNullSafety;
            HashCodeDataMemberKind = hashCodeDataMemberKind;
            HashCodeBaseCall = hashCodeBaseCall;
        }

        /// <summary>
        /// Determines the <see cref="DataMemberKind"/> of the <see cref="object.Equals(object)"/> method generation.
        /// </summary>
        public DataMemberKind EqualsDataMemberKind { get; }

        /// <summary>
        /// Determines the <see cref="DataMemberKind"/> of the <see cref="object.GetHashCode()"/> method generation.
        /// </summary>
        public DataMemberKind HashCodeDataMemberKind { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.Equals() is called.
        /// </summary>
        public BaseCall EqualsBaseCall { get; }

        /// <summary>
        /// Determines if <see langword="base"/>.GetHashCode() is called.
        /// </summary>
        public BaseCall HashCodeBaseCall { get; }

        /// <summary>
        /// Determines if the equalization is generated nullsafe.
        /// </summary>
        public NullSafety EqualsNullSafety { get; }

        DataMemberKind IAutoEqualsAttribute.DataMemberKind => EqualsDataMemberKind;

        DataMemberKind IAutoHashCodeAttribute.DataMemberKind => HashCodeDataMemberKind;

        BaseCall IAutoEqualsAttribute.BaseCall => EqualsBaseCall;

        BaseCall IAutoHashCodeAttribute.BaseCall => HashCodeBaseCall;

        NullSafety IAutoEqualsAttribute.NullSafety => EqualsNullSafety;
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
        public EqualsAndHashCodeAttribute(NullSafety equalsNullSafety = NullSafety.Auto) 
        {
            EqualsNullSafety = equalsNullSafety;
        }

        /// <summary>
        /// Determines if the equalization is generated nullsafe.
        /// </summary>
        public NullSafety EqualsNullSafety { get; }

        NullSafety IEqualsAttribute.NullSafety => EqualsNullSafety;
    }

    /// <summary>
    /// Combines attributes <see cref="EqualsExcludeAttribute"/> and <see cref="HashCodeExcludeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute, IEqualsExcludeAttribute, IHashCodeExcludeAttribute { }
}
