using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, 
        Inherited = false, AllowMultiple = false)]
    public class EqualsAttribute : Attribute, IEqualsAttribute
    {
        public EqualsAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }
}
