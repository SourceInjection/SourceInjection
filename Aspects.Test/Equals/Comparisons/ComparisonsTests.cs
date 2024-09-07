using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparisons
{
    [TestFixture]
    public class ComparisonsTests
    {
        private const string propertyName = "Property";


        private static bool UsesOperatorEqualization(IMethod m, string name) 
            => m.Body.Contains(Equalization.Operator(name));


        [Test]
        [TestCaseSource(
            typeof(ClassWithProperties_ThatArePropertiesWithDefaultEqualOperators),
            nameof(ClassWithProperties_ThatArePropertiesWithDefaultEqualOperators.Properties))]
        public void PropertyEqualization_WithType_ThatHasEqualOperatorByDefault_UsesEqualOperator(string propertyName)
        {
            var sut = EqualsMethod.FromType<ClassWithProperties_ThatArePropertiesWithDefaultEqualOperators>();
            Assert.That(UsesOperatorEqualization(sut, propertyName));
        }

        [Test]
        public void EqualizationWorksAsExpected()
        {
            var a = new ClassEmpty();
            var b = new ClassEmpty();

            Assert.That(a, Is.EqualTo(b));
        }

        [Test]
        [TestCaseSource(
            typeof(ClassWithCollections_ThatAreEquatableBySequenceEqual),
            nameof(ClassWithCollections_ThatAreEquatableBySequenceEqual.Properties))]
        public void PropertyEqualization_WithCollection_ThatIsComparableBySequenceEqual_UsesLinqSequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ClassWithCollections_ThatAreEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(Equalization.LinqCollection(propertyName, false)));
        }

        [Test]
        [TestCaseSource(
            typeof(ClassWithCollections_ThatAreNotEquatableBySequenceEqual),
            nameof(ClassWithCollections_ThatAreNotEquatableBySequenceEqual.Properties))]
        public void PropertyEqualization_WithCollection_ThatIsNotComparableBySequenceEqual_UsesAspectsSequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ClassWithCollections_ThatAreNotEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(Equalization.AspectsCollection(propertyName, false)));
        }


        [Test]
        [TestCaseSource(
            typeof(ClassWithMultiDimensionalArrays),
            nameof(ClassWithMultiDimensionalArrays.Properties))]
        public void PropertyEqualization_WithMultiDimensionalArray_UsesAspectsArraySequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ClassWithMultiDimensionalArrays>();
            Assert.That(sut.Body.Contains(Equalization.AspectsArray(propertyName, false)));
        }

        [Test]
        public void PropertyEqualization_WithCollection_ThatIsNullableAndCanBeComparedWithSequenceEqual_UsesNullsafeLinqSequenceEqual()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableCollection_ThatIsEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(Equalization.LinqCollection(propertyName, true)));
        }
    }
}
