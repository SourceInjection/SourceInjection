using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        [Test]
        [TestCaseSource(typeof(ComparerResources), nameof(ComparerResources.Types))]
        public void ComparerEqualization_UsesComparer(Type type)
        {
            var sut = EqualsMethod.FromType(type);
            var comparerCode = Equalization.Comparer(type, "Property", false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithExternComparers_DetectsNullable()
        {

        }
    }
}
