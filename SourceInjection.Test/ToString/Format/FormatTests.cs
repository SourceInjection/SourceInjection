using CompileUnits.CSharp;
using NUnit.Framework;

namespace SourceInjection.Test.ToString.Format
{
    [TestFixture]
    internal class FormatTests
    {
        const string format = "HH:mm";
        const string propertyName = "Property";

        private static void TestFormat(Type type)
        {
            var memberCode = Test.ToString.ToString.Member(propertyName, null, format);
            var expectedCode = Test.ToString.ToString.Body(type, memberCode);

            var sut = ToStringMethod.FromType(type);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassWithCustomFormatedMember_ContainsFormatInToString()
        {
            TestFormat(typeof(ClassWithCustomFormat));
        }

        [Test]
        public void ClassWithCustomFormatedNullableMember_DoesNotThrowIfNull()
        {
            var sut = new ClassWithCustomFormatAtNullableMember();

            Assert.DoesNotThrow(() => sut.ToString());
        }
    }
}
