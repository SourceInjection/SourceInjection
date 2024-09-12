using CompileUnits.CSharp;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Aspects.Test.HashCode.Comparer
{
    [TestFixture]
    internal class ComparerTests
    {
        [Test]
        public void ClassWithIntPropertyAndNonNullableComparer_IsNotNullSafe()
        {
            var sut = HashCodeMethod.FromType<ClassWithIntPropertyAndNonNullableComparer>();
            var type = typeof(ClassWithIntPropertyAndNonNullableComparer);
            var property = nameof(ClassWithIntPropertyAndNonNullableComparer.Int);
            var memberHash = HashCode.Comparer(type, property, false);

            var expectedCode = HashCode.CombineMethodBody(type, memberHashs: memberHash);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }


        [Test]
        public void ClassWithIntPropertyAndNullableComparer_IsNotNullSafe()
        {
            var sut = HashCodeMethod.FromType<ClassWithIntPropertyAndNullableComparer>();
            var type = typeof(ClassWithIntPropertyAndNullableComparer);
            var property = nameof(ClassWithIntPropertyAndNullableComparer.Int);
            var memberHash = HashCode.Comparer(type, property, false);

            var expectedCode = HashCode.CombineMethodBody(type, memberHashs: memberHash);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassWithNullableIntPropertyAndNullableComparer_IsNotNullSafe()
        {
            var sut = HashCodeMethod.FromType<ClassWithNullableIntPropertyAndNullableComparer>();
            var type = typeof(ClassWithNullableIntPropertyAndNullableComparer);
            var property = nameof(ClassWithNullableIntPropertyAndNullableComparer.Int);
            var memberHash = HashCode.Comparer(type, property, false);

            var expectedCode = HashCode.CombineMethodBody(type, memberHashs: memberHash);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }

        [Test]
        public void ClassWithNullableIntPropertyAndNonNullableComparer_IsNullSafe()
        {
            var sut = HashCodeMethod.FromType<ClassWithNullableIntPropertyAndNonNullableComparer>();
            var type = typeof(ClassWithNullableIntPropertyAndNonNullableComparer);
            var property = nameof(ClassWithNullableIntPropertyAndNonNullableComparer.Int);
            var memberHash = HashCode.Comparer(type, property, true);

            var expectedCode = HashCode.CombineMethodBody(type, memberHashs: memberHash);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
