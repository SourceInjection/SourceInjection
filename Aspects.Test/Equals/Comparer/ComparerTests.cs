using CompileUnits.CSharp;
using NUnit.Framework;
using System.Reflection;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        [Test]
        public void PropertyEqualization_WithCustomComparer_UsesComparer()
        {
            const string propertyName = "Property";

            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer>();
            var comparerName = typeof(ClassWithMember_ThatHasCustomComparer).FullName + "."
                + typeof(ClassWithMember_ThatHasCustomComparer)
            .GetNestedTypes(BindingFlags.NonPublic).First().Name;

            Assert.That(sut.Body.Contains($"&& new {comparerName}().Equals({propertyName}, #i.{propertyName});"));
        }
    }
}
