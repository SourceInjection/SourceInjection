using System;
using System.Linq;
using System.Reflection;

namespace SourceInjection.Util
{
    internal static class ParameterInfoExtensions
    {
        public static bool SupportsNullable(this ParameterInfo parameter)
        {
            return !parameter.CustomAttributes.Any(a => a.IsDisallowNullAttribute())
                && (Nullable.GetUnderlyingType(parameter.ParameterType) != null || parameter.CustomAttributes.Any(a => a.IsAllowNullAttribute()));
        }
    }
}
