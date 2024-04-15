using Aspects.Attributes;


namespace Aspects.Test.Common
{
    [ToString]
    [HashCode]
    [Equals]
    public partial class Data
    {
        private int _int;

        public int Int => _int;

        public object? Object { get; }

    }
}
