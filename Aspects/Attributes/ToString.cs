using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoToStringAttribute : Attribute, IToStringConfigAttribute
    {
        public AutoToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringAttribute : Attribute, IToStringAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ToStringExcludeAttribute : Attribute, IToStringExcludeAttribute { }
}
