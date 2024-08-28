using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.CompileTime.Equals.Comparisons
{
    [TestFixture]
    public class ComparisonsTests
    {
        private const string propertyName = "Property";
        private static readonly string Equalization = $"&& {propertyName}.Equals(#i.{propertyName})";
        private static readonly string NullSafeEqualization = $"&& ({propertyName} == null && #i.{propertyName} == null " +
                $"|| {propertyName}?.Equals(#i.{propertyName}) == true)";


        private static bool IsNullSafe(IMethod m) => m.Body.Contains(NullSafeEqualization);

        private static bool IsNotNullSafe(IMethod m) => m.Body.Contains(Equalization);

        private static bool DoesNotCallBase(IMethod m) => !m.Body.Contains("base");

        private static bool UsesOperatorEqualization(IMethod m, string propertyName)
        {
            return m.Body.Contains($"&& {propertyName} == #i.{propertyName}");
        }

        private static bool CallsBase(IMethod m)
        {
            var argName = m.Parameters[0].Name;
            return m.Body.Contains($"&& base.Equals({argName})");
        }


        [Test]
        public void ClassTypeEqualization_ContainsReferenceEqualization()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeEmpty>();
            var paramName = sut.Parameters[0].Name;
            Assert.That(sut.Body.Contains($"return {paramName} == this || {paramName} is {nameof(ReferenceTypeEmpty)}"));
        }

        [Test]
        public void StructTypeEqualization_DoesNotContainPointerEqualization_ButContainsTypeCheck()
        {
            var sut = EqualsMethod.FromType<NonReferenceTypeEmpty>();
            var paramName = sut.Parameters[0].Name;
            Assert.That(sut.Body.Contains($"return {paramName} is {nameof(NonReferenceTypeEmpty)}"));
        }

        [Test]
        public void NullablePropertyEqualization_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithNullableProperty>();
            Assert.That(IsNullSafe(sut));
        }
        [Test]
        public void PropertyEqualization_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty>();
            Assert.That(IsNotNullSafe(sut));
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty_NullableDisabled>();
            Assert.That(IsNullSafe(sut));
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_ButNullCheckIsOff_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty_NullableDisabled_NullSafetyOff>();
            Assert.That(IsNotNullSafe(sut));
        }

        [Test]
        public void NullablePropertyEqualization_WhenNullCheckIsOn_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty_NullSafetyOn>();
            Assert.That(IsNullSafe(sut));
        }

        [Test]
        public void ClassTypeEmptyEqualization_DoesNotContainVarDefinition()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeEmpty>();
            var paramName = sut!.Parameters[0].Name;
            var expectedBody = $"{{ return {paramName} == this || {paramName} is {nameof(ReferenceTypeEmpty)}; }}";

            Assert.That(sut.Body.IsEquivalentTo(expectedBody));
        }

        [Test]
        public void StructTypeEmptyEqualization_DoesNotContainVarDefinition()
        {
            var sut = EqualsMethod.FromType<NonReferenceTypeEmpty>();
            var paramName = sut!.Parameters[0].Name;
            var expectedBody = $"{{ return {paramName} is {nameof(NonReferenceTypeEmpty)}; }}";

            Assert.That(sut.Body.IsEquivalentTo(expectedBody));
        }

        [Test]
        public void ClassTypeEqualization_WithBaseThatHasAutoEquals_CallsBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeEmptyWithBase_ThatHasAutoEquals>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void ClassTypeEqualization_WithBaseThatHasAutoEquals_WithBaseCallOff_DoesNotCallBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeEmptyWithBase_ThatHasAutoEquals_WithBaseCallOff>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassTypeEqualization_WithNoEqualsOverrideInSuper_WithBaseCallOn_CallsBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeEmpty_WithBaseCallOn>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void StructTypeEqualization_WithBaseCallOn_DoesNotCallBase()
        {
            var sut = EqualsMethod.FromType<NonReferenceTypeEmpty_WithBaseCallOn>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassTypeEqualization_WithBaseThatHasEqualsAttributeOnMember_CallsBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithBase_ThatHasEqualsOnMember>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void ClassTypeEqualization_WithBaseThatHasNoEqualsOverride_DoesNotCallBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithBase_ThatHasNoEqualsOverride>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassTypeEqualization_WithBaseThatHasEqualsOverride_CallsBaseEquals()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithBase_ThatHasEqualsOverride>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void PropertyEqualization_NotNullAttribute_NullableDisabled_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty_ThatHasNotNullAttribute_NullableDisabled>();
            Assert.That(IsNotNullSafe(sut));
        }


        [Test]
        public void PropertyEqualization_WithMaybeNullAttribute_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperty_ThatHasMaybeNullAttribute>();
            Assert.That(IsNullSafe(sut));
        }

        [Test]
        [TestCaseSource(
            typeof(ReferenceTypeWithProperties_ThatAllArePropertiesWithDefaultEqualOperators),
            nameof(ReferenceTypeWithProperties_ThatAllArePropertiesWithDefaultEqualOperators.Properties))]
        public void PropertyEqualization_WithType_ThatHasEqualOperatorByDefault_UsesEqualOperator(string propertyName)
        {
            var sut = EqualsMethod.FromType<ReferenceTypeWithProperties_ThatAllArePropertiesWithDefaultEqualOperators>();
            Assert.That(UsesOperatorEqualization(sut, propertyName));
        }

        [Test]
        public void EqualizationWorksAsExpected()
        {
            var a = new ReferenceTypeEmpty();
            var b = new ReferenceTypeEmpty();

            Assert.That(a, Is.EqualTo(b));
        }
    }
}
