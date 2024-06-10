using Aspects.Attributes;
using Aspects.SourceGenerators.Common;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.CompileTime.PropertyEvents.PropertyChanged
{
    internal class PropertyChangedTests
    {
        private static IEnumerable<FieldInfo> GetFields()
        {
            return typeof(PropertyEventTestClass).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.GetCustomAttribute<NotifyPropertyChangedAttribute>() is not null);
        }


        [Test]
        public void SutHasPropertyForeachPropertyChangedAttribute()
        {
            var properties = typeof(PropertyEventTestClass).GetProperties()
                .Select(p => p.Name)
                .ToHashSet();

            Assert.Multiple(() =>
            {
                var fields = GetFields();
                Assert.That(fields.Count(), Is.GreaterThan(0));

                foreach (var field in fields)
                    Assert.That(properties, Does.Contain(CodeSnippets.PropertyNameFromField(field.Name)));
            });
        }
    }
}
