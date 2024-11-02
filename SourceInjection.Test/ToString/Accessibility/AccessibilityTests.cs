using CompileUnits.CSharp;
using NUnit.Framework;

namespace SourceInjection.Test.ToString.Accessibility
{
    [TestFixture]
    internal class AccessibilityTests
    {
        [Test]
        [TestCaseSource(typeof(AccessibilityResources), nameof(AccessibilityResources.TestConfigs))]
        public void TestAccessibility(Type type, Accessibilities accessibility)
        {
            var included = AccessibilityResources.GetTargetedFields(accessibility)
                .Select(m => Test.ToString.ToString.Member(m))
                .ToArray();

            var expectedBody = Test.ToString.ToString.Body(type, included);

            var sut = ToStringMethod.FromType(type);

            Assert.That(sut.Body.IsEquivalentTo(expectedBody));
        }
    }
}
