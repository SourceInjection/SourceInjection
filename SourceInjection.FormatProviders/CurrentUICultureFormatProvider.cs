using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class CurrentUICultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.CurrentUICulture.GetFormat(formatType);
    }
}
