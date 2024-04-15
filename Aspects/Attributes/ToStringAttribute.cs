using Aspects.Attributes.Base;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ToStringAttribute : BasicOverrideMethodAttribute
    {
        public ToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.Public, params string[] excludedMembers)
            : base(dataMemberKind, excludedMembers)
        { }
    }
}
