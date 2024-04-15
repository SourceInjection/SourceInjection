using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class HashCodeAttribute : BasicOverrideMethodAttribute
    {
        public HashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.All, params string[] excludedMembers)
            : base(dataMemberKind, excludedMembers)
        { }
    }
}
