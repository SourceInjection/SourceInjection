using System.Globalization;
using System;

namespace SourceInjection.FormatProviders
{
    internal class CurrentNumberFormatProvider : IFormatProvider
    {
        public object GetFormat(Type formatType) => NumberFormatInfo.CurrentInfo.GetFormat(formatType);
    }
}
