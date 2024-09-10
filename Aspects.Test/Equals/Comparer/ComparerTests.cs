using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        const string propertyName = "Property";
        const string comparerName = "IntComparer";

        private static Action BuildEqualization<TType>() => BuildEqualization<TType>(propertyName);

        private static Action BuildEqualization<TType>(string propertyName)
        {
            return () =>
            {
                var lhs = (TType?)Activator.CreateInstance(typeof(TType));
                var rhs = (TType?)Activator.CreateInstance(typeof(TType));

                var prop = typeof(TType).GetProperty(propertyName);

                if (lhs is null || rhs is null || prop is null)
                    throw new InvalidOperationException($"could not get necessary information.");

                prop.SetValue(lhs, null);
                prop.SetValue(rhs, null);

                lhs.Equals(rhs);
            };
        }

        [Test]
        public void ComparerEqualization_WithMemberConfig_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_EqualsConfig>();
            var comparerCode = Equalization.Comparer(comparerName, propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithComparerAttribute_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_ComparerAttribute>();
            var comparerCode = Equalization.Comparer(comparerName, propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithNonNullableComparer_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithNullableNonReferenceTypeMember_ThatHasNonNullableComparer>();
            Assert.DoesNotThrow(() => equalization());
        }
    }
}
