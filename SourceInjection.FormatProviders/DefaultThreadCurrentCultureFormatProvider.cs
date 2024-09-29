using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class DefaultThreadCurrentCultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.DefaultThreadCurrentCulture?.GetFormat(formatType);
    }
}
