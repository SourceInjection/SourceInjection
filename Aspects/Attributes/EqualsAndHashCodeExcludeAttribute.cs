using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class EqualsAndHashCodeExcludeAttribute : Attribute
    { }
}
