using NUnit.Framework;

namespace Aspects.Test.Equals.NullSafety
{
    internal class NullSafetyTests
    {
        private const string propertyName = "Property";

        private static Action BuildEqualization<TType>() 
        {
            return Equalization.Build<TType>(propertyName);
        }


        [Test]
        public void NullablePropertyEqualization_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithNullableProperty>();
            Assert.DoesNotThrow(() => equalization());
        }

        [Test]
        public void PropertyEqualization_IsNotNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty>();
            Assert.Throws<NullReferenceException>(() => equalization());
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty_NullableDisabled>();
            Assert.DoesNotThrow(() => equalization());
        }

        [Test]
        public void PropertyEqualization_WhenNullableFeatureDisabled_ButNullCheckIsOff_IsNotNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty_NullableDisabled_NullSafetyOff>();
            Assert.Throws<NullReferenceException>(() => equalization());
        }

        [Test]
        public void NullablePropertyEqualization_WhenNullCheckIsOn_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty_NullSafetyOn>();
            Assert.DoesNotThrow(() => equalization());
        }

        [Test]
        public void PropertyEqualization_NotNullAttribute_NullableDisabled_IsNotNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty_ThatHasNotNullAttribute_NullableDisabled>();
            Assert.Throws<NullReferenceException>(() => equalization());
        }

        [Test]
        public void PropertyEqualization_WithMaybeNullAttribute_IsNullSafe()
        {
            var equalization = BuildEqualization<ClassWithProperty_ThatHasMaybeNullAttribute>();
            Assert.DoesNotThrow(() => equalization());
        }
    }
}
