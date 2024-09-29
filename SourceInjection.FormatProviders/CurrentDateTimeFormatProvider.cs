using System;
using System.Globalization;

namespace SourceInjection.FormatProviders
{
    public class CurrentDateTimeFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => DateTimeFormatInfo.CurrentInfo.GetFormat(formatType);
    }
}
