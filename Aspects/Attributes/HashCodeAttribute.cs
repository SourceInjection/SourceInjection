using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class HashCodeAttribute : BaseOverrideMethodAttribute
    {
        public HashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.All)
            : base(dataMemberKind)
        { }

        internal static HashCodeAttribute FromAttributeData(AttributeData data)
        {
            return new HashCodeAttribute((DataMemberKind)data.ConstructorArguments[0].Value);
        }
    }
}
