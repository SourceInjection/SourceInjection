using System;
using System.Globalization;

namespace SourceInjection.FormatProviders
{
    internal class CurrentDateTimeFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => DateTimeFormatInfo.CurrentInfo.GetFormat(formatType);
    }
}
