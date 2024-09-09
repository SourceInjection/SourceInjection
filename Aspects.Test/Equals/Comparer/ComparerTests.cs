using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        const string propertyName = "Property";

        [Test]
        public void ComparerEqualization_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer>();
            var comparerCode = Equalization.Comparer("IntComparer", propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithNullableNonReferenceType_WithComparerThatDoesNotSupportNullable_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer>();
            var comparerCode = Equalization.ComparerNullableNonReferenceType("IntComparer", propertyName, true);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithNullableNonReferenceType_WithComparerThatDoesSupportNullable_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer>();
            var comparerCode = Equalization.ComparerNullableNonReferenceType("IntComparer", propertyName, false);

            Assert.That(sut.Body.Contains($"&& {comparerCode}"));
        }
    }
}
