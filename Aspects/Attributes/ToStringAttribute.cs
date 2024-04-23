using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, 
        Inherited = false, AllowMultiple = false)]
    public class ToStringAttribute : Attribute, IToStringAttribute
    {
        public ToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember)
        { 
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }
}
