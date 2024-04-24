using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAndHashCodeAttribute : Attribute, IEqualsConfigAttribute, IHashCodeConfigAttribute
    {
        public AutoEqualsAndHashCodeAttribute(DataMemberKind dataMemberKind = DataMemberKind.ProjectConfig)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeAttribute : Attribute, IEqualsAttribute, IHashCodeAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute, IEqualsExcludeAttribute, IHashCodeExcludeAttribute { }
}
