using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.HashCode.Comparer
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.Types))]
        public void ComparerEqualization_UsesComparer(Type type)
        {
            var sut = HashCodeMethod.FromType(type);
            var comparerCode = HashCode.Comparer(type, "Property", false);

            Assert.That(sut.Body.Contains(comparerCode));
        }
    }
}
