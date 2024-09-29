using System;
using System.Globalization;

namespace SourceInjection.FormatProviders
{
    public class CurrentCultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.CurrentCulture.GetFormat(formatType);
    }
}
