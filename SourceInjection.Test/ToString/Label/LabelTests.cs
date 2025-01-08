using CompileUnits.CSharp;
using NUnit.Framework;

namespace SourceInjection.Test.ToString.Label
{
    [TestFixture]
    internal class LabelTests
    {
        const string label = "Label";
        const string propertyName = "Property";

        private static void TestLabel(Type type)
        {
            var memberCode = Test.ToString.ToString.Member(type, propertyName, label);
            var expectedCode = Test.ToString.ToString.Body(type, memberCode);

            var sut = ToStringMethod.FromType(type);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ToStringWithMember_ThatHasLabel_GeneratesToStringWithLabel()
        {
            TestLabel(typeof(ClassWithMemberThatHasLabel));
        }
    }
}
