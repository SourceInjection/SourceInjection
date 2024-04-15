using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class HashCodeAttribute : BaseOverrideMethodAttribute
    {
        public HashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.All, params string[] excludedMembers)
            : base(dataMemberKind, excludedMembers)
        { }

        internal static HashCodeAttribute FromAttributeData(AttributeData data)
        {
            return new HashCodeAttribute(
                (DataMemberKind)data.ConstructorArguments[0].Value,
                (string[])data.ConstructorArguments[1].Value);
        }
    }
}
