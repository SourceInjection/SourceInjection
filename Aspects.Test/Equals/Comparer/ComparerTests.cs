using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        const string propertyName = "Property";
        const string comparerName = "Comparer";

        private static Action BuildEqualization<TType>() 
            => Equalization.Build<TType>(propertyName);


        [Test]
        public void ComparerEqualization_WithMemberConfig_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_EqualsConfig>();
            var comparerCode = Equalization.Comparer(typeof(ClassWithMember_ThatHasCustomComparer_EqualsConfig), propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithComparerAttribute_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_ComparerAttribute>();
            var comparerCode = Equalization.Comparer(typeof(ClassWithMember_ThatHasCustomComparer_ComparerAttribute), propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithNonNullableComparer_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer>();
            Assert.DoesNotThrow(() => equalization());
        }

        [Test]
        public void ComparerEqualization_WithNonNullableComparer_WithNullableNonReferenceProperty_WithNullSafetyOff_IsNotNullSafe()
        {
            var equalization = BuildEqualization<ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer_WithNullsafetyOff>();
            Assert.Throws<InvalidOperationException>(() => equalization());
        }

        [Test]
        public void ComparerEqualization_WithNullableComparer_WithNullableNonReferenceProperty_WithNullSafetyOn_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer_WithNullsafetyOn>();
            var comparerCode = Equalization.Comparer(typeof(ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer_WithNullsafetyOn), propertyName, true);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithNonNullableComparer_WithNullableNonReferenceProperty_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer>();
            var type = typeof(ClassWithNullableNonReferenceTypeMember_ThatHasNullableComparer);
            var comparerCode = Equalization.Comparer(type, propertyName, false);
            var nullSafeCode = Equalization.Comparer(type, propertyName, true);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(comparerCode));
                Assert.That(!sut.Body.Contains(nullSafeCode));
            });
        }

        [Test]
        public void ComparerEqualization_WithNullableComparer_WithNullableProperty_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableMember_ThatHasNullableComparer>();
            var type = typeof(ClassWithNullableMember_ThatHasNullableComparer);
            var comparerCode = Equalization.Comparer(type, propertyName, false);
            var nullSafeCode = Equalization.Comparer(type, propertyName, true);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(comparerCode));
                Assert.That(!sut.Body.Contains(nullSafeCode));
            });
        }
    }
}
