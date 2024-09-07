using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.HashCode.BaseCall
{
    [TestFixture]
    internal class BaseCallTests
    {
        [Test]
        public void ClassTypeEmpty_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<ClassEmpty>();
            var expectedCode = HashCode.CombineMethodBody(typeof(ClassEmpty));

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassTypeEmpty_WithBaseCallOn_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<ClassEmpty_WithBaseCallOn>();
            var expectedCode = HashCode.CombineMethodBody(typeof(ClassEmpty_WithBaseCallOn), true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void StructTypeEmpty_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<StructEmpty>();
            var expectedCode = HashCode.CombineMethodBody(typeof(StructEmpty));

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void StructTypeEmpty_WithBaseCallOn_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<StructEmpty_WithBaseCallOn>();
            var expectedCode = HashCode.CombineMethodBody(typeof(StructEmpty_WithBaseCallOn));

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithDefaultConfig_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty>();
            var expectedCode = HashCode.CombineMethodBody(typeof(SubClassOfClassEmpty), true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithBaseCallOn_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty_WithBaseCallOn>();
            var expectedCode = HashCode.CombineMethodBody(typeof(SubClassOfClassEmpty_WithBaseCallOn), true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithBaseCallOff_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty_WithBaseCallOff>();
            var expectedCode = HashCode.CombineMethodBody(typeof(SubClassOfClassEmpty_WithBaseCallOff));

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
