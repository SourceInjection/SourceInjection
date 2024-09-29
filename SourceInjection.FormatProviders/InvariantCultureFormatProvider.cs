using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class InvariantCultureFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => CultureInfo.InvariantCulture.GetFormat(formatType);
    }
}
