using Aspects.Interfaces;
using System;

namespace Aspects
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.ToString"/> generation.<br/>
    /// All members with <see cref="ToStringExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="ToStringAttribute"/>.<br/>
    /// On default properties and fields are automaticly merged by scanning the getter code in properties.<br/>
    /// Linked fields can only be detected when the property fullfilles the following grammar:<br/>
    /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoToStringAttribute : Attribute, IAutoToStringAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="ToStringAttribute"/>.
        /// </summary>
        /// <param name="dataMemberKind">
        /// Defines which data members are used to generate <see cref="object.ToString"/>.<br/>
        /// With <see cref="DataMemberKind.Field"/> only fields are used.<br/>
        /// With <see cref="DataMemberKind.Property"/> only properties are used.<br/>
        /// The default value is <see cref="DataMemberKind.DataMember"/> 
        /// which includes fields and properties and automaticly merges linked fields in properties by scanning the getter code.
        /// Linked fields can only be detected when the property fullfilles the following grammar:
        /// <include file="Comments.xml" path="doc/members/member[@name='Properties:PropertySyntax']/*"/>
        /// </param>
        public AutoToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember, Accessibility accessibility = Accessibility.Public)
        {
            DataMemberKind = dataMemberKind;
            Accessibility = accessibility;
        }

        /// <summary>
        /// Defines which data members are used to generate <see cref="object.ToString"/>.
        /// </summary>
        public DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Defines which accessibility is required to be used foreach member to be included to generate <see cref="object.ToString"/>.
        /// </summary>
        public Accessibility Accessibility { get; }
    }

    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.ToString"/> generation.<br/>
    /// All members with this attribute will be used to generate <see cref="object.ToString"/>.<br/>
    /// If used in combination with <see cref="AutoToStringAttribute"/> this attribute includes members that are normally excluded.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringAttribute : Attribute, IToStringAttribute 
    {
        public ToStringAttribute(string label = null, string format = null)
        {
            Label = label;
            Format = format;
        }

        /// <summary>
        /// Defines the name which is used to represent the member.
        /// </summary>
        public string Label { get; }

        /// <summary>
        /// Defines the format which will be handover to the string representation of the targeted object.
        /// </summary>
        public string Format { get; }
    }

    /// <summary>
    /// Must be used in combination with <see cref="AutoToStringAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringExcludeAttribute : Attribute, IToStringExcludeAttribute { }
}
