using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.Equals.Code
{
    [TestFixture]
    public class ComparisonsTests
    {
        private const string propertyName = "Property";

        private static bool IsNullSafe(IMethod m, string name = propertyName) 
            => m.Body.Contains(EqualsMethod.NullSafeEqualsEqualization(name));

        private static bool IsNotNullSafe(IMethod m, string name = propertyName) 
            => m.Body.Contains(EqualsMethod.EqualsEqualization(name));

        private static bool DoesNotCallBase(IMethod m) 
            => !m.Body.Contains("base");

        private static bool UsesOperatorEqualization(IMethod m, string name) 
            => m.Body.Contains(EqualsMethod.OperatorEqualization(name));

        private static bool CallsBase(IMethod m)
            => m.Body.Contains($"&& base.Equals({m.Parameters[0].Name})");

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

        [Test]
        [TestCaseSource(
            typeof(ReferenceType_WithCollectionsEquatableBySequenceEqual),
            nameof(ReferenceType_WithCollectionsEquatableBySequenceEqual.Properties))]
        public void PropertyEqualization_WithCollection_ThatIsComparableBySequenceEqual_UsesLinqSequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ReferenceType_WithCollectionsEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(EqualsMethod.LinqCollectionEqualization(propertyName)));
        }

        [Test]
        [TestCaseSource(
            typeof(ReferenceType_WithCollectionsNotEquatableBySequenceEqual),
            nameof(ReferenceType_WithCollectionsNotEquatableBySequenceEqual.Properties))]
        public void PropertyEqualization_WithCollection_ThatIsNotComparableBySequenceEqual_UsesAspectsSequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ReferenceType_WithCollectionsNotEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(EqualsMethod.AspectsCollectionEqualization(propertyName)));
        }


        [Test]
        [TestCaseSource(
            typeof(ReferenceType_WithMultiDimensionalArrays),
            nameof(ReferenceType_WithMultiDimensionalArrays.Properties))]
        public void PropertyEqualization_WithMultiDimensionalArray_UsesAspectsArraySequenceEqual(string propertyName)
        {
            var sut = EqualsMethod.FromType<ReferenceType_WithMultiDimensionalArrays>();
            Assert.That(sut.Body.Contains(EqualsMethod.AspectsArrayEqualization(propertyName)));
        }

        [Test]
        public void PropertyEqualization_WithCollection_ThatIsNullableAndCanBeComparedWithSequenceEqual_UsesNullsafeLinqSequenceEqual()
        {
            var sut = EqualsMethod.FromType<ReferenceType_WithNullableCollectionEquatableBySequenceEqual>();
            Assert.That(sut.Body.Contains(EqualsMethod.NullSafeLinqCollectionEqualization(propertyName)));
        }

        [Test]
        public void PropertyEqualization_WithCustomComparer_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ReferenceType_WithMemberThatHasCustomComparer>();
            var comparerName = typeof(ReferenceType_WithMemberThatHasCustomComparer).FullName + "."
                + typeof(ReferenceType_WithMemberThatHasCustomComparer)
                    .GetNestedTypes(BindingFlags.NonPublic).First().Name;

            Assert.That(sut.Body.Contains($"&& new {comparerName}().Equals({propertyName}, #i.{propertyName});"));
        }
    }
}
