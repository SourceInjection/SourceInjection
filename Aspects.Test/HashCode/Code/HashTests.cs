using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.HashCode.Code
{
    [TestFixture]
    internal class HashTests
    {
        [Test]
        public void ClassTypeEmpty_DoesNotCallBase()
        {
            var sut = HashCodeMethod.FromType<ClassEmpty>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(ClassEmpty).FullName!);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassType_WhichInheritsFromClassTypeEmpty_DoesCallBase()
        {
            var sut = HashCodeMethod.FromType<SubClassFromClassEmpty>();
            var expectedCode = HashCodeMethod.HashCodeCombine(typeof(SubClassFromClassEmpty).FullName!, true);

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
    }
}
