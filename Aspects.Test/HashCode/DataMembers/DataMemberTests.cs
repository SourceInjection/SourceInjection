using Aspects.SourceGeneration;
using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.HashCode.DataMembers
{
    internal class DataMemberTests
    {
        private const string propertyName = "Property";

        private static (IMethod Method, string PropertyName) GetTypeInfo<T>(string fieldName)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            return (HashCodeMethod.FromType<T>(),
                typeof(T).GetProperty(Snippets.PropertyNameFromField(fieldName), flags)!.Name);
        }

        private static (IMethod Method, string FieldName, string PropertyName) GetTypeInfo<T>()
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;

            return (HashCodeMethod.FromType<T>(),
                typeof(T).GetField("_int", flags)!.Name,
                typeof(T).GetProperty("Int", flags)!.Name);
        }


        [Test]
        public void ClassEqualization_WithDataMemberKindDataMember_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_DataMember>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(field));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindField_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Field>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(field));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(property));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_WithArrowFunctionProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(property));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_WithArrowFunctionPropertyThatUsesCoalesceOperator_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(property));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindProperty_UsesProperty()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Property>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(property));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberKindField_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_Field>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(field));
                Assert.That(!sut.Body.Contains(property));
            });
        }

        [Test]
        public void ClassEqualization_WithGeneratedPropertyAndDataMemberDataMember_UsesField()
        {
            var (sut, field, property) = GetTypeInfo<ClassWithGeneratedProperty_DataMemberKind_DataMember>();
            Assert.Multiple(() =>
            {
                Assert.That(sut.Body.Contains(field));
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
                Assert.That(sut.Body.Contains(property));
                Assert.That(!sut.Body.Contains(field));
            });
        }

        [Test]
        public void ClassEqualization_WithDefaultSettings_IgnoresQueryProperty()
        {
            var sut = HashCodeMethod.FromType<ClassWithQueryProperty_WithDefaultSettings>();
            Assert.That(!sut.Body.Contains(propertyName));
        }

        [Test]
        public void ClassEqualization_WithDataMemberKindProperty_IgnoresQueryProperty()
        {
            var sut = HashCodeMethod.FromType<ClassWithQueryProperty_WithDataMemberKindProperty>();
            Assert.That(!sut.Body.Contains(propertyName));
        }

        [Test]
        public void ClassEqualization_WithQueryProperty_WithEqualsInclude_IncludesQueryProperty()
        {
            var sut = HashCodeMethod.FromType<ClassWithQueryProperty_WithEqualsInclude>();
            Assert.That(sut.Body.Contains(propertyName));
        }

        [Test]
        public void ClassEqualization_WithDefaultSettings_IgnoresConstantField()
        {
            var sut = HashCodeMethod.FromType<ClassWithConstantField>();
            var field = nameof(ClassWithConstantField._int);
            Assert.That(!sut.Body.Contains(field));
        }

        [Test]
        public void ClassEqualization_IgnoresConstantField_EvenIfIncluded()
        {
            var sut = HashCodeMethod.FromType<ClassWithConstantField_WithEqualsInclude>();
            var field = nameof(ClassWithConstantField_WithEqualsInclude._int);
            Assert.That(!sut.Body.Contains(field));
        }

        [Test]
        public void ClassEqualization_WithDefaultSettings_IgnoresStaticField()
        {
            var sut = HashCodeMethod.FromType<ClassWithStaticField>();
            var field = typeof(ClassWithStaticField).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Single().Name;
            Assert.That(!sut.Body.Contains(field));
        }

        [Test]
        public void ClassEqualization_IgnoresStaticFiel_EvenIfIncluded()
        {
            var sut = HashCodeMethod.FromType<ClassWithStaticField_WithEqualsInclude>();
            var field = typeof(ClassWithStaticField_WithEqualsInclude).GetFields(BindingFlags.Static | BindingFlags.NonPublic).Single().Name;
            Assert.That(!sut.Body.Contains(field));
        }

        [Test]
        public void ClassEqualization_IgnoresEvents()
        {
            var sut = HashCodeMethod.FromType<ClassWithEvent>();
            var ev = typeof(ClassWithEvent).GetEvents().Single().Name;
            Assert.That(!sut.Body.Contains(ev));
        }
    }
}
