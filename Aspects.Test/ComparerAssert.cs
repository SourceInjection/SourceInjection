using NUnit.Framework;
using CompileUnits.CSharp;
using Aspects.Test.Equals;

namespace Aspects.Test
{
    internal static class ComparerAssert
    {
        public static void NullSafety(Type type, bool nullSafe, Func<Type, string, bool, string> comparerMethod)
        {
            var sut = EqualsMethod.FromType(type);
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
