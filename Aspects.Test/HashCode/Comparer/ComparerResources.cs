using System.Diagnostics.CodeAnalysis;

#pragma warning disable
namespace Aspects.Test.HashCode.Comparer
{
    public partial class ClassWithIntPropertyAndDefaultComparer
    {
        private class Comparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode([DisallowNull] int obj)
            {
                throw new NotImplementedException();
            }
        }

        [HashCode(typeof(Comparer))]
        public int Int { get; }
    }


    public partial class ClassWithNullableIntPropertyAndDefaultComparer
    {
        private class Comparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                throw new NotImplementedException();
            }

            public int GetHashCode([DisallowNull] int obj)
            {
                throw new NotImplementedException();
            }
        }

        [HashCode(typeof(Comparer))]
        public int? Int { get; }


        public void DoStuff()
        {
            int res = Int != null ? new Comparer().GetHashCode(Int.Value) : 0;
        }
    }
}

#pragma warning enable
