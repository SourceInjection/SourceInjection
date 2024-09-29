using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class InvariantCultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.InvariantCulture.GetFormat(formatType);
    }
}
