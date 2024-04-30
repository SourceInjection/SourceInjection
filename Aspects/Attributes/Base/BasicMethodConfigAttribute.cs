using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes.Base
{
    public abstract class BasicMethodConfigAttribute : Attribute, IBasicMethodConfigAttribute
    {
        protected BasicMethodConfigAttribute(DataMemberKind dataMemberKind)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }
}
