using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer.ReferenceType.NullSafetyAuto
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.Types))]
        public void AssertDefaultComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.NullSafety(type, nullSafe, Equalization.Comparer);
        }
    }
}
