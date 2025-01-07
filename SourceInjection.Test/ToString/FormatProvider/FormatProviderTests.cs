using CompileUnits.CSharp;
using NUnit.Framework;
using SourceInjection.Test.FormatProviders;

namespace SourceInjection.Test.ToString.FormatProvider
{
    [TestFixture]   
    internal class FormatProviderTests
    {
        const string propertyName = "Property";
        private static readonly string formatProvider = typeof(CurrentCultureFormatProvider).FullName!;

        [Test]
        public void ToString_WithFormatProvider_UsesFormatProvider()
        {
            var sut = ToStringMethod.FromType<ClassWithFormatProvider>();
            var expectedCode = $"{propertyName}.ToString(null, new {formatProvider}())";

            Assert.That(sut.Body.Contains(expectedCode));
        }

        [Test]
        public void ToString_WithFormatAndFormatProvider_UsesFormatAndFormatProvider()
        {
            var sut = ToStringMethod.FromType<ClassWithFormatAndFormatProvider>();
            var expectedCode = $"{propertyName}.ToString(\"HH:mm\", new {formatProvider}())";

            Assert.That(sut.Body.Contains(expectedCode));
        }

        [Test]
        public void ToString_WithFormatAndFormatProvider_WhereFormatIsStringEmpty_PassesStringEmpty()
        {
            var sut = ToStringMethod.FromType<ClassWithFormatAndFormatProviderWhereFormatIsStringEmpty>();
            var expectedCode = $"{propertyName}.ToString(\"\", new {formatProvider}())";

            Assert.That(sut.Body.Contains(expectedCode));
        }

        [Test]
        [TestCaseSource(typeof(FormatProviderResources), nameof(FormatProviderResources.MustBeNullSafe))]
        public void Test_Nullsafety(Type type)
        {
            var sut = ToStringMethod.FromType(type);
            var expectedCode = $"{propertyName}?.ToString(null, new {formatProvider}())";

            Assert.That(sut.Body.Contains(expectedCode));
        }
    }
}
