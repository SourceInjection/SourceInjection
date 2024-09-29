using SourceInjection.FormatProviders;

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

    internal partial class ClassWithFormatProvider
    {
        [ToString(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProvider
    {
        [ToString(format: "HH:mm", formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithFormatAndFormatProviderWhereFormatIsStringEmpty
    {
        [ToString(format: "", formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime Property { get; }
    }

    internal partial class ClassWithNullableStructAndFormatProvider
    {
        [ToString(formatProvider: typeof(CurrentCultureFormatProvider))]
        public DateTime? Property { get; }
    }

    internal partial class ClassWithObjectAndFormatProvider
    {
        [ToString(formatProvider: typeof(CurrentCultureFormatProvider))]
        public IFormattable Property { get; } = null!;
    }
}
