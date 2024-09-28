using NUnit.Framework;

namespace SourceInjection.Test.Equals.NullSafety
{
    [TestFixture]
    internal class NullSafetyTests
    {
        private const string propertyName = "Property";

        private static Action BuildEqualization(Type type) 
            => Equalization.Build(type, propertyName);


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
    }
}
