using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    /// <summary>
    /// <see cref="Attribute"/> for automatic <see cref="object.ToString"/> generation.<br/>
    /// All members with <see cref="ToStringExcludeAttribute"/> are excluded.<br/>
    /// All members that are normally excluded can be included by placing a <see cref="ToStringAttribute"/>.<br/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoToStringAttribute : Attribute, IAutoToStringAttribute
    {
        public AutoToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember, Accessibility accessibility = Accessibility.Public)
        {
            DataMemberKind = dataMemberKind;
            Accessibility = accessibility;
        }

        public DataMemberKind DataMemberKind { get; }

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

        public string Label { get; }

        public string Format { get; }
    }

    /// <summary>
    /// Must be used in combination with <see cref="AutoToStringAttribute"/>.
    /// Excludes attributed members that are normally included.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringExcludeAttribute : Attribute, IToStringExcludeAttribute { }
}
