using Aspects.Attributes;


namespace Aspects.Test.Common
{
    [ToString]
    [EqualsAndHashCode]
    public partial class Data : DataBase
    {
        private int _int;

        public int Int => _int;

        public object? Object { get; }

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    [ToString]
    [EqualsAndHashCode]
    public abstract partial class DataBase
    {
        public abstract string Name { get; set; }
    }
}
