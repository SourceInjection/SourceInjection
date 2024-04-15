using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ToStringAttribute : BaseOverrideMethodAttribute
    {
        public ToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.Public, params string[] excludedMembers)
            : base(dataMemberKind, excludedMembers)
        { }

        internal static ToStringAttribute FromAttributeData(AttributeData data)
        {
            return new ToStringAttribute(
                (DataMemberKind)data.ConstructorArguments[0].Value,
                (string[])data.ConstructorArguments[1].Value);
        }
    }
}
