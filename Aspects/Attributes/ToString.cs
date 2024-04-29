using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoToStringAttribute : Attribute, IToStringConfigAttribute
    {
        public AutoToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember, bool collectionsEnabled = false)
        {
            DataMemberKind = dataMemberKind;
            CollectionsEnabled = collectionsEnabled;
        }

        public DataMemberKind DataMemberKind { get; }

        public bool CollectionsEnabled { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringAttribute : Attribute, IToStringAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringExcludeAttribute : Attribute, IToStringExcludeAttribute { }
}
