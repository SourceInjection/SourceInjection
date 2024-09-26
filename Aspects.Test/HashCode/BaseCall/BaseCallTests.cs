using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.HashCode.BaseCall
{
    [TestFixture]
    internal class BaseCallTests
    {
        [Test]
        [TestCaseSource(typeof(BaseCallResources), nameof(BaseCallResources.MustCallBase))]
        public void HashCode_CallsBase(Type type)
        {
            var sut = HashCodeMethod.FromType(type);
            var expectedCode = HashCode.CombineMethodBody(type, true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        [TestCaseSource(typeof(BaseCallResources), nameof(BaseCallResources.MustNotCallBase))]
        public void HashCode_DoesNotCallBase(Type type)
        {
            var sut = HashCodeMethod.FromType(type);
            var expectedCode = HashCode.CombineMethodBody(type, false);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
