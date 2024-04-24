using Aspects.Attributes;
using System.Collections;


namespace Aspects.Test.Common
{
    [AutoEqualsAndHashCode]
    public partial class Data : DataBase
    {
        object? _obj;
        object? _obj2;

        private int _int;

        public int Int => _int;

        public object? Object { get; }

        public List<List<string>> Strings2d { get; set; }

        public IList List { get; set; }

        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public object? Object1 { get => _obj; }

        public object? Object2 
        {
            get 
            {
                var value = _obj2;
                _obj2 = value;

                return _obj2;
            } 
        }

        public object? Object3 { get; }

        public object? Object4 { get; }

        public object? Object5 { get; }

    }

    [AutoEqualsAndHashCode]
    public abstract partial class DataBase
    {
        public abstract string Name { get; set; }
    }
}
