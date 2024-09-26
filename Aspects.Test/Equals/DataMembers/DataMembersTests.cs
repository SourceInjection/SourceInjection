using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.DataMembers
{
    [TestFixture]
    internal class DataMembersTests
    {
        private const string fieldName = "_field";
        private const string propertyName = "Property";


        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustUseField))]
        public void Equalization_MustUseField(Type type)
        {
            var sut = EqualsMethod.FromType(type);
            Assert.That(sut.Body.Contains(fieldName));
        }

        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustUseProperty))]
        public void Equalization_MustUseProperty(Type type)
        {
            var sut = EqualsMethod.FromType(type);
            Assert.That(sut.Body.Contains(propertyName));
        }


        [Test]
        [TestCaseSource(typeof(DataMemberResources), nameof(DataMemberResources.MustBeIgnored))]
        public void Equalization_MustBeIgnored(Type type)
        {
            var expectedCode = Equalization.Body(type);
            var sut = EqualsMethod.FromType(type);

            Assert.That(sut.Body.IsEquivalentTo(expectedCode));
        }
    }
}
