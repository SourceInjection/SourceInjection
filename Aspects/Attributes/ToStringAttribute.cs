using System;
using System.Collections.Generic;
using System.Text;

namespace Aspects.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    internal class ToStringAttribute : Attribute
    { }
}
