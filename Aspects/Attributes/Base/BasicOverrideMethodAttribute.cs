
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes.Base
{
    public class BasicOverrideMethodAttribute : Attribute
    {
        protected BasicOverrideMethodAttribute(DataMemberKind dataMemberKind) 
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }

        internal static BasicOverrideMethodAttribute FromAttributeData(AttributeData data)
        {
            var kind = (DataMemberKind)data.ConstructorArguments[0].Value;
            return new BasicOverrideMethodAttribute(kind);
        }
    }
}
