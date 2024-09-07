using CompileUnits.CSharp;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Aspects.Test.HashCode.Comparer
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        public void ClassWithIntPropertyAndCustomComparer_UsesComparer()
        {
            var sut = HashCodeMethod.FromType<ClassWithIntPropertyAndDefaultComparer>();
            var property = nameof(ClassWithIntPropertyAndDefaultComparer.Int);
            var memberHash = HashCode.Comparer(typeof(ClassWithIntPropertyAndDefaultComparer), property, false);

            var expectedCode = HashCode.CombineMethodBody(typeof(ClassWithIntPropertyAndDefaultComparer), memberHashs: memberHash);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
