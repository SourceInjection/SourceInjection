using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    public class InvariantNumberFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => NumberFormatInfo.InvariantInfo.GetFormat(formatType);
    }
}
