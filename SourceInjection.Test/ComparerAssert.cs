using NUnit.Framework;
using CompileUnits.CSharp;
using SourceInjection.Test.Equals;
using SourceInjection.Test.HashCode;

namespace SourceInjection.Test
{
    internal static class ComparerAssert
    {

        public static void HashCodeNullSafety(Type type, bool nullSafe, Func<Type, string, bool, string> comparerMethod)
        {
            var sut = HashCodeMethod.FromType(type);
            NullSafety(type, nullSafe, comparerMethod, sut);
        }

        public static void EqualsNullSafety(Type type, bool nullSafe, Func<Type, string, bool, string> comparerMethod)
        {
            var sut = EqualsMethod.FromType(type);
            NullSafety(type, nullSafe, comparerMethod, sut);
        }

        private static void NullSafety(Type type, bool nullSafe, Func<Type, string, bool, string> comparerMethod, IMethod sut)
        {
            var expectedMethod = comparerMethod(type, "Property", nullSafe);

            if (nullSafe)
                Assert.That(sut.Body.Contains(expectedMethod));
            else
            {
                var notExpected = comparerMethod(type, "Property", !nullSafe);

                Assert.Multiple(() =>
                {
                    Assert.That(sut.Body.Contains(expectedMethod));
                    Assert.That(!sut.Body.Contains(notExpected));
                });
            }
        }
    }
}
