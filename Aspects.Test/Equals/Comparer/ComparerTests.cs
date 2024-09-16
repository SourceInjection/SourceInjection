using CompileUnits.CSharp;
using NUnit.Framework;

namespace Aspects.Test.Equals.Comparer
{
    internal class ComparerTests
    {
        const string propertyName = "Property";
        const string comparerName = "Comparer";

        private static Action BuildEqualization<TType>() 
            => Equalization.Build<TType>(propertyName);


        [Test]
        public void ComparerEqualization_WithMemberConfig_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_EqualsConfig>();
            var comparerCode = Equalization.Comparer(typeof(ClassWithMember_ThatHasCustomComparer_EqualsConfig), propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }

        [Test]
        public void ComparerEqualization_WithComparerAttribute_UsesComparer()
        {
            var sut = EqualsMethod.FromType<ClassWithMember_ThatHasCustomComparer_ComparerAttribute>();
            var comparerCode = Equalization.Comparer(typeof(ClassWithMember_ThatHasCustomComparer_ComparerAttribute), propertyName, false);

            Assert.That(sut.Body.Contains(comparerCode));
        }
    }
}
