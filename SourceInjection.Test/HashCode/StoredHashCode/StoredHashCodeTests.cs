using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace SourceInjection.Test.HashCode.StoredHashCode
{
    [TestFixture]
    internal class StoredHashCodeTests
    {
        private const string StoredHashCode = "_storedHashCode";

        private static FieldInfo? GetStoredHashCodeField<T>() => typeof(T).GetField(StoredHashCode, BindingFlags.NonPublic | BindingFlags.Instance);

        [Test]
        public void ClassEmpty_DoesNotGeneratePrivateField()
        {
            var field = GetStoredHashCodeField<ClassEmpty>();
            Assert.That(field, Is.Null);
        }

        [Test]
        public void ClassWithStoredHashCode_GeneratesPrivateField()
        {
            var field = GetStoredHashCodeField<ClassEmptyWithStoredHashCode>()!;

            Assert.Multiple(() =>
            {
                Assert.That(field.Name, Is.EqualTo(StoredHashCode));
                Assert.That(field.FieldType, Is.EqualTo(typeof(int?)));
            });
        }

        [Test]
        public void ClassWithStoredHashCode_GeneratesOptimizedMethod()
        {
            var sut = HashCodeMethod.FromType<ClassEmptyWithStoredHashCode>();
            var expectedMethod = HashCode.CombineMethodBody(typeof(ClassEmptyWithStoredHashCode), false, true);

            Assert.That(sut.Body.IsEquivalentTo(expectedMethod));
        }

        [Test]
        public void ClassWith10MembersAndStoredHashCode_GeneratesOptimizedMethod()
        {
            var sut = HashCodeMethod.FromType<ClassWith10Members_WithStoredHashCode>();
            var members = typeof(ClassWith10Members_WithStoredHashCode).GetProperties().Select(p => p.Name).ToArray();
            var expectedMethod = HashCode.SequencialMethodBody(typeof(ClassWith10Members_WithStoredHashCode), false, true, members);

            Assert.That(sut.Body.IsEquivalentTo(expectedMethod));
        }
    }
}
