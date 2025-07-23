using NUnit.Framework;
using SourceInjection.Test.Util.Extensions;

namespace SourceInjection.Test.PropertyEvents.PropertyChanged.Accessibility
{
    [TestFixture]
    internal class AccessibilityTests
    {
        [Test]
        [TestCaseSource(typeof(AccessibilityResources), nameof(AccessibilityResources.ExpectedAccessibilities))]
        public void GeneratedProperty_HasExpectedAccessibility(Type type, AccessModifier accessModifier)
        {
            var sut = Property.FromType(type, "Property");

            Assert.That(sut.Setter.AccessModifier, Is.EqualTo(accessModifier.ToCompileUnitsAccessModifier()));
        }
    }
}
