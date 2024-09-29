using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class InvariantNumberFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => NumberFormatInfo.InvariantInfo.GetFormat(formatType);
    }
}
