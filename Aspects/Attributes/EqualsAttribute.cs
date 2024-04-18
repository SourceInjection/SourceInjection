using Aspects.Attributes.Base;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, 
        Inherited = false, AllowMultiple = false)]
    public class EqualsAttribute : BasicOverrideMethodAttribute
    {
        public EqualsAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember) 
            : base(dataMemberKind)
        { }
    }
}
