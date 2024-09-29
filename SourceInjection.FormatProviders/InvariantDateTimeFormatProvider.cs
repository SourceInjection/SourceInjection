using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class InvariantDateTimeFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => DateTimeFormatInfo.InvariantInfo.GetFormat(formatType);
    }
}
