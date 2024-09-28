using NUnit.Framework;

namespace SourceInjection.Test.HashCode.Comparer.ReferenceType.NullSafetyAuto
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.Types))]
        public void AssertDefaultComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.HashCodeNullSafety(type, nullSafe, HashCode.Comparer);
        }
    }
}
