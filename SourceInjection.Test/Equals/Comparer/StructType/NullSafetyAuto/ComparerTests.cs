using NUnit.Framework;

namespace SourceInjection.Test.Equals.Comparer.StructType.NullSafetyAuto
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.MustUseComparerEqualization))]
        public void AssertDefaultComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.EqualsNullSafety(type, nullSafe, Equalization.Comparer);
        }

        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.MustUseComparerStructTypeEqualization))]
        public void AssertStructComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.EqualsNullSafety(type, nullSafe, Equalization.ComparerNullableNonReferenceType);
        }
    }
}
