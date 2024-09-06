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
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(ClassEmpty).FullName!);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassTypeEmpty_WithBaseCallOn_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<ClassEmpty_WithBaseCallOn>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(ClassEmpty_WithBaseCallOn).FullName!, true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void StructTypeEmpty_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<StructEmpty>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(StructEmpty).FullName!);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void StructTypeEmpty_WithBaseCallOn_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<StructEmpty_WithBaseCallOn>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(StructEmpty_WithBaseCallOn).FullName!);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithDefaultConfig_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(SubClassOfClassEmpty).FullName!, true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithBaseCallOn_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty_WithBaseCallOn>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(SubClassOfClassEmpty_WithBaseCallOn).FullName!, true);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void SubClassOfClassEmpty_WithBaseCallOff_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassOfClassEmpty_WithBaseCallOff>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(SubClassOfClassEmpty_WithBaseCallOff).FullName!);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
