using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class AutoEqualsAttribute : Attribute, IEqualsConfigAttribute
    {
        public AutoEqualsAttribute(DataMemberKind dataMemberKind = DataMemberKind.ProjectConfig)
        {
            DataMemberKind = dataMemberKind;
        }

        public DataMemberKind DataMemberKind { get; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAttribute : Attribute, IEqualsAttribute { }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsExcludeAttribute : Attribute, IEqualsExcludeAttribute { }
}
