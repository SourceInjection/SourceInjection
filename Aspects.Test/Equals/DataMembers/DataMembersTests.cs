using Aspects.SourceGenerators.Common;
using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.Equals.DataMembers
{
    [TestFixture]
    internal class DataMembersTests
    {
        private static (IMethod Method, string PropertyName) GetTypeInfo<T>(string fieldName)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            return (EqualsMethod.FromType<T>(),
                typeof(T).GetProperty(SourceGenerators.Common.Code.PropertyNameFromField(fieldName), flags)!.Name);
        }

        private static (IMethod Method, string FieldName, string PropertyName) GetTypeInfo<T>()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            return (EqualsMethod.FromType<T>(),
                typeof(T).GetField("_int", flags)!.Name,
                typeof(T).GetProperty("Int", flags)!.Name);
        }


        [Test]
        public void ClassEqualization_WithDataMemberKindDataMember_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_DataMember>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(field)));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindField_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Field>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(field)));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(property)));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_WithArrowFunctionProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(property)));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_WithArrowFunctionPropertyThatUsesCoalesceOperator_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_ArrowFunction_CoalesceOperator_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(property)));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(property)));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindField_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Field>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(field)));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberDataMember_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_DataMember>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.MemberOperatorEqualization(field)));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindProperty_UsesFieldConfiguration()
        {
            var field = "_object";
            var (sut, property) = GetTypeInfo<ClassWithGeneratedProperty_WithFieldConfiguration>(field);

            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(EqualsMethod.NullSafeMemberEqualization(property)));
                Assert.That(!sut.Body.Contains(field));
            });
        }
    }
}
