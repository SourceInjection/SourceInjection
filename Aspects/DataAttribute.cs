using Aspects.Interfaces;
using System;

namespace Aspects
{
    public class DataAttribute : Attribute, IAutoEqualsAttribute, IAutoHashCodeAttribute, IAutoToStringAttribute
    {
        public DataAttribute(
            DataMemberKind dataMemberKind = DataMemberKind.DataMember,
            NullSafety nullSafety = NullSafety.Auto,
            BaseCall baseCall = BaseCall.Auto, 
            bool storeHashCode = false)
        {
            DataMemberKind = dataMemberKind;
            NullSafety = nullSafety;
            BaseCall = baseCall;
            StoreHashCode = storeHashCode;
        }

        public DataMemberKind DataMemberKind { get; }

        public NullSafety NullSafety { get; }

        public BaseCall BaseCall { get; }

        public bool StoreHashCode { get; }
    }
}
