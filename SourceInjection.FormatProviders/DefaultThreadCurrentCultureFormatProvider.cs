using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class DefaultThreadCurrentCultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.DefaultThreadCurrentCulture?.GetFormat(formatType);
    }
}
