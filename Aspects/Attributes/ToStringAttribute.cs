using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class ToStringAttribute : BaseOverrideMethodAttribute
    {
        public ToStringAttribute(DataMemberKind dataMemberKind = DataMemberKind.Property)
            : base(dataMemberKind)
        { }

        internal static ToStringAttribute FromAttributeData(AttributeData data)
        {
            return new ToStringAttribute((DataMemberKind)data.ConstructorArguments[0].Value);
        }
    }
}
