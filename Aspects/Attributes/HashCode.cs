using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoHashCodeAttribute : Attribute, IHashCodeConfigAttribute
    {
        public AutoHashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.ProjectConfig)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class HashCodeAttribute : Attribute, IHashCodeAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class HashCodeExcludeAttribute : Attribute, IHashCodeExcludeAttribute { }
}
