using CompileUnits.CSharp;
using NUnit.Framework;

namespace SourceInjection.Test.Equals.Comparer
{
    [TestFixture]
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
    }
}
