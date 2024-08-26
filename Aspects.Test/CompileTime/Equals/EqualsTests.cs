using Aspects.SourceGenerators;
using CompileUnits.CSharp;
using NUnit.Framework;
using CompileUnit = Aspects.Test.CodeAnalysis.CompileUnit;

namespace Aspects.Test.CompileTime.Equals
{
    [TestFixture]
    public class EqualsTests
    {
        private static IMethod? GetSut<T>()
        {
            var cu = CompileUnit.FromGeneratedCode<EqualsSourceGenerator, T>();

            return cu.AllChildren()
                .OfType<IMethod>()
                .SingleOrDefault(IsEqualsMethod);
        }

        private static string NullSafeEqualization(string propertyName)
        {
            const string other = "other";
            return $"{propertyName} == null && {other}.{propertyName} == null || {propertyName}?.Equals({other}.{propertyName}) == true";
        }

        private static string Equalization(string propertyName)
        {
            const string other = "other";
            return $"{propertyName}.Equals({other}.{propertyName})";
        }

        private static bool IsEqualsMethod(IMethod m)
        {
            return m.Name == nameof(Equals)
                && m.Parameters.Count == 1
                && m.Parameters[0].Type.FormatedText is "object" or "object?";
        }

        [Test]
        public void ReferenceTypeEquals_ContainsPointerEqualization()
        {
            var sut = GetSut<ReferenceType>();
            var paramName = sut!.Parameters[0].Name;

            Assert.That(sut.Body.Contains($"return {paramName} == this || {paramName} is {nameof(ReferenceType)}"));
        }

        [Test]
        public void NonReferenceType_DoesNotContainPointerEqualizationButContainsTypeCheck()
        {
            var sut = GetSut<NonReferenceType>();
            var paramName = sut!.Parameters[0].Name;

            Assert.That(sut.Body.Contains($"return {paramName} is {nameof(NonReferenceType)}"));
        }

        [Test]
        public void NullablePropertyEqualization_IsNullSafe()
        {
            var sut = GetSut<ReferenceTypeWithNullableProperty>();
            var prop = nameof(ReferenceTypeWithNullableProperty.Property);

            Assert.That(sut!.Body.Contains(NullSafeEqualization(prop)));
        }
        [Test]
        public void NotNullablePropertyEqualization_IsNotNullSafe()
        {
            var sut = GetSut<ReferenceTypeWithNonNullableProperty>();
            var prop = nameof(ReferenceTypeWithNonNullableProperty.Property);

            Assert.That(sut!.Body.Contains(Equalization(prop)));
        }

        [Test]
        public void WhenNullableFeatureDisabled_PropertyEqualization_IsNullSafe()
        {
            var sut = GetSut<ReferenceTypeWithPropertyNullableDisabled>();
            var prop = nameof(ReferenceTypeWithPropertyNullableDisabled.Property);

            Assert.That(sut!.Body.Contains(NullSafeEqualization(prop)));
        }
    }
}
