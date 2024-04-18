using Aspects.Attributes.Base;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property, 
        Inherited = false, AllowMultiple = false)]
    public class HashCodeAttribute : BasicOverrideMethodAttribute
    {
        public HashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.DataMember)
            : base(dataMemberKind)
        { }
    }
}
