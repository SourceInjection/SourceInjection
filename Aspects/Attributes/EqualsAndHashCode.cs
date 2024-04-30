using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    /// <summary>
    /// Combines attributes <see cref="AutoEqualsAttribute"/> and <see cref="AutoHashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAndHashCodeAttribute : Attribute, IEqualsConfigAttribute, IHashCodeConfigAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="AutoEqualsAndHashCodeAttribute"/>.
        /// </summary>
        /// <param name="dataMemberKind">
        /// Defines which data members are used to generate <see cref="object.Equals(object)"/> and <see cref="object.GetHashCode"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        public AutoEqualsAndHashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }

    /// <summary>
    /// Combines attributes <see cref="EqualsAttribute"/> and <see cref="HashCodeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeAttribute : Attribute, IEqualsAttribute, IHashCodeAttribute { }

    /// <summary>
    /// Combines attributes <see cref="EqualsExcludeAttribute"/> and <see cref="HashCodeExcludeAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute, IEqualsExcludeAttribute, IHashCodeExcludeAttribute { }
}
