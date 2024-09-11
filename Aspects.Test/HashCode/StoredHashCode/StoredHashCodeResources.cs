
#pragma warning disable

namespace Aspects.Test.HashCode.StoredHashCode
{
    [AutoHashCode]
    public partial class ClassEmpty { }

    [AutoHashCode(storeHashCode: true)]
    public partial class ClassEmptyWithStoredHashCode { }

    [AutoHashCode(storeHashCode: true)]
    public partial class ClassWith10Members_WithStoredHashCode
    {
        public int Member0 { get; }

        public int Member1 { get; }

        public int Member2 { get; }

        public int Member3 { get; }

        public int Member4 { get; }

        public int Member5 { get; }

        public int Member6 { get; }

        public int Member7 { get; }

        public int Member8 { get; }

        public int Member9 { get; }
    }
}

#pragma warning restore
