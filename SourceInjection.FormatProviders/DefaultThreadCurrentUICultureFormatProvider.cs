using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class DefaultThreadCurrentUICultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.DefaultThreadCurrentUICulture?.GetFormat(formatType);
    }
}
