using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class CurrentUICultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.CurrentUICulture.GetFormat(formatType);
    }
}
