using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.TypeHandling
{
    internal class TypeHandlingTests
    {
        [Test]
        public void ClassTypeEmptyEqualization_DoesNotContainVarDefinition()
        {
            var sut = EqualsMethod.FromType<ClassEmpty>();
            var paramName = sut!.Parameters[0].Name;
            var expectedBody = $"{{ return {paramName} == this || {paramName} is {nameof(ClassEmpty)}; }}";

            Assert.That(sut.Body.IsEquivalentTo(expectedBody));
        }

        [Test]
        public void StructTypeEmptyEqualization_DoesNotContainVarDefinition()
        {
            var sut = EqualsMethod.FromType<StructEmpty>();
            var paramName = sut!.Parameters[0].Name;
            var expectedBody = $"{{ return {paramName} is {nameof(StructEmpty)}; }}";

            Assert.That(sut.Body.IsEquivalentTo(expectedBody));
        }

        [Test]
        public void ClassTypeEqualization_ContainsReferenceEqualization()
        {
            var sut = EqualsMethod.FromType<ClassEmpty>();
            var paramName = sut.Parameters[0].Name;
            Assert.That(sut.Body.Contains($"return {paramName} == this || {paramName} is {nameof(ClassEmpty)}"));
        }

        [Test]
        public void StructTypeEqualization_DoesNotContainPointerEqualization_ButContainsTypeCheck()
        {
            var sut = EqualsMethod.FromType<StructEmpty>();
            var paramName = sut.Parameters[0].Name;
            Assert.That(sut.Body.Contains($"return {paramName} is {nameof(StructEmpty)}"));
        }
    }
}
