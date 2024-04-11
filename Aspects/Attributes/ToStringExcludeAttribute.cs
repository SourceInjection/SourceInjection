using System;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal class ToStringExcludeAttribute : Attribute
    { }
}
