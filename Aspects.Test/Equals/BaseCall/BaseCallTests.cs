using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.BaseCall
{
    internal class BaseCallTests
    {
        private static bool DoesNotCallBase(IMethod m)
            => !m.Body.Contains("base");

        private static bool CallsBase(IMethod m)
            => m.Body.Contains($"&& base.Equals({m.Parameters[0].Name})");

        [Test]
        [TestCaseSource(typeof(BaseCallResources), nameof(BaseCallResources.MustCallBase))]
        public void Equals_CallsBase(Type type)
        {
            var sut = EqualsMethod.FromType(type);
            Assert.That(CallsBase(sut));
        }

        [Test]
        [TestCaseSource(typeof(BaseCallResources), nameof(BaseCallResources.MustNotCallBase))]
        public void Equals_DoesNotCallBase(Type type)
        {
            var sut = EqualsMethod.FromType(type);
            Assert.That(DoesNotCallBase(sut));
        }
    }
}
