using System;

namespace SourceInjection.Test.FormatProviders
{
#pragma warning disable S1118 // Utility classes should not have public constructors
    public class FormatProviderFactory
#pragma warning restore S1118 // Utility classes should not have public constructors
    {
        public static IFormatProvider Method()
            => new CurrentCultureFormatProvider();

        public static IFormatProvider Property => new CurrentCultureFormatProvider();
    }
}
