using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class InstalledUICultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.InstalledUICulture.GetFormat(formatType);
    }
}
