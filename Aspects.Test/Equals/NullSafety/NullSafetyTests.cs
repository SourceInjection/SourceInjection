using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.NullSafety
{
    internal class NullSafetyTests
    {
        private const string propertyName = "Property";

        private static bool IsNullSafe(IMethod m, string name = propertyName)
            => m.Body.Contains(EqualsMethod.NullSafeEqualsEqualization(name));

        private static bool IsNotNullSafe(IMethod m, string name = propertyName)
            => m.Body.Contains(EqualsMethod.EqualsEqualization(name));

        [Test]
        public void NullablePropertyEqualization_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithNullableProperty>();
            Assert.That(IsNullSafe(sut));
        }
        [Test]
        public void PropertyEqualization_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty>();
            Assert.That(IsNotNullSafe(sut));
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty_NullableDisabled>();
            Assert.That(IsNullSafe(sut));
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_ButNullCheckIsOff_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty_NullableDisabled_NullSafetyOff>();
            Assert.That(IsNotNullSafe(sut));
        }

        [Test]
        public void NullablePropertyEqualization_WhenNullCheckIsOn_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty_NullSafetyOn>();
            Assert.That(IsNullSafe(sut));
        }

        [Test]
        public void PropertyEqualization_NotNullAttribute_NullableDisabled_IsNotNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty_ThatHasNotNullAttribute_NullableDisabled>();
            Assert.That(IsNotNullSafe(sut));
        }


        [Test]
        public void PropertyEqualization_WithMaybeNullAttribute_IsNullSafe()
        {
            var sut = EqualsMethod.FromType<ClassWithProperty_ThatHasMaybeNullAttribute>();
            Assert.That(IsNullSafe(sut));
        }
    }
}
