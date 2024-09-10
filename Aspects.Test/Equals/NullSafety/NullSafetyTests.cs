using NUnit.Framework;

namespace Aspects.Test.Equals.NullSafety
{
    internal class NullSafetyTests
    {
        private const string propertyName = "Property";

        private static Action BuildEqualization<TType>() 
        {
            return BuildEqualization<TType>(propertyName);
        }

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
