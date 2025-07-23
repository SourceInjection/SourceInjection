using NUnit.Framework;

namespace SourceInjection.Test.Equals.NullSafety
{
    [TestFixture]
    internal class NullSafetyTests
    {
        private const string propertyName = "Property";

        private static Func<bool> BuildEqualization(Type type, object? lhsValue = null, object? rhsValue = null) 
            => Equalization.Build(type, propertyName, lhsValue, rhsValue);


        [Test]
        [TestCaseSource(typeof(NullSafetyResources), nameof(NullSafetyResources.MustBeNullSafe))]
        public void Equalization_MustBeNullSafe(Type type)
        {
            var equalization = BuildEqualization(type);
            Assert.DoesNotThrow(() => equalization());
        }

        [Test]
        [TestCaseSource(typeof(NullSafetyResources), nameof(NullSafetyResources.MustNotBeNullSafe))]
        public void Equalization_MustNotBeNullSafe(Type type)
        {
            var equalization = BuildEqualization(type);
            Assert.Throws<NullReferenceException>(() => equalization());
        }

        [Test]
        public void Equalization_NullSafetyWorksAsExpected()
        {
            var type = typeof(ClassWithNullableProperty);

            Assert.Multiple(() =>
            {
                Assert.That(BuildEqualization(type, new DataClass(), new DataClass())(), Is.True);
                Assert.That(BuildEqualization(type, new DataClass() { Value = 1 }, new DataClass() { Value = 1})(), Is.True);
                Assert.That(BuildEqualization(type, new DataClass() { Value = 1 }, new DataClass() { Value = 42 })(), Is.False);
                Assert.That(BuildEqualization(type, new DataClass(), null)(), Is.False);
                Assert.That(BuildEqualization(type, null, new DataClass())(), Is.False);
                Assert.That(BuildEqualization(type, null, null)(), Is.True);
            });
        }
    }
}
