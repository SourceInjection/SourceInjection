using SourceInjection.Test.FormatProviders;
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
        [FormatProvider(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProvider
    {
        [ToString(format: "HH:mm")]
        [FormatProvider(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProviderWhereFormatIsStringEmpty
    {
        [ToString(format: "")]
        [FormatProvider(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    [AutoToString]
    internal partial class ClassWithNullableStructAndFormatProvider
    {
        [FormatProvider(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime? Property { get; }
    }

    [AutoToString]
    internal partial class ClassWithObjectAndFormatProvider
    {
        [FormatProvider(formatProvider: typeof(CurrentCultureFormatProvider))]
        public IFormattable Property { get; } = null!;
    }

    [AutoToString]
    internal partial class ClassWithFactoryPropertyFormatProvider
    {
        [FormatProvider(staticClass: typeof(CultureInfo), factoryMember: nameof(CultureInfo.CurrentCulture))]
        public DateTime? Property { get; }
    }

    [AutoToString]
    internal partial class ClassWithFactoryMethodFormatProvider
    {
        [FormatProvider(staticClass: typeof(FormatProviderFactory), factoryMember: nameof(FormatProviderFactory.Method))]
        public DateTime? Property { get; }
    }
}
