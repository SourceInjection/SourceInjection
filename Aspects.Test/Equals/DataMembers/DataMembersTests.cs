using Aspects.Test.Equals;
using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.Equals.DataMembers
{
    [TestFixture]
    internal class DataMembersTests
    {
        private static (IMethod Method, string FieldName, string PropertyName) GetTypeInfo<T>()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            return (EqualsMethod.FromType<T>(),
                typeof(T).GetField("_int", flags)!.Name,
                typeof(T).GetProperty("Int", flags)!.Name);
        }

        private static string Comparison(string memberName) => $"&& {memberName} == #i.{memberName}";

        [Test]
        public void ClassEqualization_WithDataMemberKindDataMember_UsesField()
        {
            var (sut, field, _) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_DataMember>();
            Assert.That(sut.Body.Contains(Comparison(field)));
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindField_UsesField()
        {
            var (sut, field, _) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Field>();
            Assert.That(sut.Body.Contains(Comparison(field)));
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_UsesProperty()
        {
            var (sut, _, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Property>();
            Assert.That(sut.Body.Contains(Comparison(property)));
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindProperty_UsesProperty()
        {
            var (sut, _, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Property>();
            Assert.That(sut.Body.Contains(Comparison(property)));
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindField_UsesField()
        {
            var (sut, field, _) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Field>();
            Assert.That(sut.Body.Contains(Comparison(field)));
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberDataMember_UsesField()
        {
            var (sut, field, _) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_DataMember>();
            Assert.That(sut.Body.Contains(Comparison(field)));
        }
    }
}
