using NUnit.Framework;

namespace SourceInjection.Test.HashCode.Comparer.StructType.NullSafetyOff
{
    [TestFixture]
    internal class ComparerTests
    {

        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.MustUseComparerEqualization))]
        public void AssertDefaultComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.HashCodeNullSafety(type, nullSafe, HashCode.Comparer);
        }

        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.MustUseComparerStructTypeEqualization))]
        public void AssertStructComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.HashCodeNullSafety(type, nullSafe, HashCode.ComparerNullableNonReferenceType);
        }
    }
}
