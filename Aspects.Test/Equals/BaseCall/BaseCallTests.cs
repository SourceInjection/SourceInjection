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
        public void ClassEqualization_WithBaseThatHasAutoEquals_CallsBase()
        {
            var sut = EqualsMethod.FromType<ClassWithBase_ThatHasAutoEquals>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void ClassEqualization_WithBaseThatHasAutoEquals_WithBaseCallOff_DoesNotCallBase()
        {
            var sut = EqualsMethod.FromType<ClassEmptyWithBase_ThatHasAutoEquals_WithBaseCallOff>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassEqualization_WithNoEqualsOverrideInSuper_WithBaseCallOn_CallsBase()
        {
            var sut = EqualsMethod.FromType<ClassEmpty_WithBaseCallOn>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void StructEqualization_WithBaseCallOn_DoesNotCallBase()
        {
            var sut = EqualsMethod.FromType<NonReferenceTypeEmpty_WithBaseCallOn>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassEqualization_WithBaseThatHasEqualsAttributeOnMember_CallsBase()
        {
            var sut = EqualsMethod.FromType<ClassWithBase_ThatHasEqualsOnMember>();
            Assert.That(CallsBase(sut));
        }

        [Test]
        public void ClassEqualization_WithBaseThatHasNoEqualsOverride_DoesNotCallBase()
        {
            var sut = EqualsMethod.FromType<ClassWithBase_ThatHasNoEqualsOverride>();
            Assert.That(DoesNotCallBase(sut));
        }

        [Test]
        public void ClassEqualization_WithBaseThatHasEqualsOverride_CallsBase()
        {
            var sut = EqualsMethod.FromType<ClassWithBase_ThatHasEqualsOverride>();
            Assert.That(CallsBase(sut));
        }
    }
}
