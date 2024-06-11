using Aspects.SourceGenerators.Common;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.CompileTime.PropertyEvents
{
    internal class PropertyEventTests
    {
        private static System.Reflection.PropertyInfo? GetProperty(string name)
        {
            return typeof(PropertyEventTestClass).GetProperties()
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }

        [Test]
        [TestCaseSource(typeof(PropertyEventTestSources), nameof(PropertyEventTestSources.AllAttributedFields))]
        public void PropertyIsGeneratedForeachAttributedField(FieldInfo field)
        {
            var generatedPropertyName = CodeSnippets.PropertyNameFromField(field.Name);
            var property = GetProperty(generatedPropertyName);

            Assert.That(property, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(property.GetMethod, Is.Not.Null);
                Assert.That(property.SetMethod, Is.Not.Null);
            });
        }
    }
}
