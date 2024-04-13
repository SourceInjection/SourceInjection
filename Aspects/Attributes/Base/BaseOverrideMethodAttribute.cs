
using System;

namespace Aspects.Attributes.Base
{
    public abstract class BaseOverrideMethodAttribute : Attribute
    {
        protected BaseOverrideMethodAttribute(DataMemberKind dataMemberKind) 
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }
}
