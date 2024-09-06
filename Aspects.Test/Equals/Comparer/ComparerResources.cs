using Aspects.Attributes;

namespace Aspects.Test.Equals.Comparer
{
    public partial class ClassWithMember_ThatHasCustomComparer
    {
        private class IntComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode(int obj)
            {
                throw new NotImplementedException();
            }
        }

        [Equals(equalityComparer: typeof(IntComparer))]
        public int Property { get; }
    }
}
