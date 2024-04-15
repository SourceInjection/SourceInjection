using Aspects.Attributes.Base;
using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class EqualsAttribute : BaseOverrideMethodAttribute
    {
        public EqualsAttribute(DataMemberKind dataMemberKind = DataMemberKind.All, params string[] excludedMembers) 
            : base(dataMemberKind, excludedMembers)
        { }

        internal static EqualsAttribute FromAttributeData(AttributeData data)
        {
            return new EqualsAttribute(
                (DataMemberKind)data.ConstructorArguments[0].Value, 
                (string[])data.ConstructorArguments[1].Value);
        }
    }
}
