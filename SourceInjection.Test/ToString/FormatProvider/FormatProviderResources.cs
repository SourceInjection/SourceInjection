using SourceInjection.Test.Util.FormatProviders;
using System.Globalization;

namespace SourceInjection.Test.ToString.FormatProvider
{
    internal static class FormatProviderResources
    {
        public static readonly Type[] MustBeNullSafe = 
        [
            typeof(ClassWithNullableStructAndFormatProvider), 
            typeof(ClassWithObjectAndFormatProvider)
        ];
    }
    
    [AutoToString]
    internal partial class ClassWithFormatProvider
    {
        [FormatProvider(type: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProvider
    {
        [ToString(format: "HH:mm")]
        [FormatProvider(type: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProviderWhereFormatIsStringEmpty
    {
        [ToString(format: "")]
        [FormatProvider(type: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    [AutoToString]
    internal partial class ClassWithNullableStructAndFormatProvider
    {
        [FormatProvider(type: typeof(CurrentCultureFormatProvider))]
        public DateTime? Property { get; }
    }

    [AutoToString]
    internal partial class ClassWithObjectAndFormatProvider
    {
        [FormatProvider(type: typeof(CurrentCultureFormatProvider))]
        public IFormattable Property { get; } = null!;
    }

    [AutoToString]
    internal partial class ClassWithFactoryPropertyFormatProvider
    {
        [FormatProvider(type: typeof(CultureInfo), property: nameof(CultureInfo.CurrentCulture))]
        public DateTime? Property { get; }
    }
}
