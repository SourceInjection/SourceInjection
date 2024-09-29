using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class CurrentNumberFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => NumberFormatInfo.CurrentInfo.GetFormat(formatType);
    }
}
