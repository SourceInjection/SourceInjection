using NUnit.Framework;

namespace SourceInjection.Test.Equals.Comparer.ReferenceType.NullSafetyOn
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.Types))]
        public void AssertDefaultComparisonNullSafety(Type type, bool nullSafe)
        {
            ComparerAssert.EqualsNullSafety(type, nullSafe, Equalization.Comparer);
        }
    }
}
