using System;

namespace SourceInjection
{
    [AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public sealed class ThrowIfNullAttribute : Attribute { }
}
