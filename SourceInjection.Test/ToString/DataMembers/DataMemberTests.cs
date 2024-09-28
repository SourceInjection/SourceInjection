using CompileUnits.CSharp;
using NUnit.Framework;

namespace SourceInjection.Test.ToString.DataMembers
{
    internal class DataMemberTests
    {
        private const string fieldName = "_field";
        private const string propertyName = "Property";

        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustUseField))]
        public void Equalization_MustUseField(Type type)
        {
            var sut = ToStringMethod.FromType(type);
            Assert.That(sut.Body.Contains(fieldName));
        }

        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustUseProperty))]
        public void Equalization_MustUseProperty(Type type)
        {
            var sut = ToStringMethod.FromType(type);
            Assert.That(sut.Body.Contains(propertyName));
        }

        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustBeIgnored))]
        public void Equalization_MustBeIgnored(Type type)
        {
            var expectedCode = Test.ToString.ToString.Body(type);
            var sut = ToStringMethod.FromType(type);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
