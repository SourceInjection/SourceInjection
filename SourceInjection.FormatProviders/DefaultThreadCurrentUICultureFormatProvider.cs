using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class DefaultThreadCurrentUICultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.DefaultThreadCurrentUICulture?.GetFormat(formatType);
    }
}
