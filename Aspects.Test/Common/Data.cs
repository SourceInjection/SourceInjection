using Aspects.Attributes;
using System.Collections;


namespace Aspects.Test.Common
{
    [AutoEqualsAndHashCode]
    public partial class Data : DataBase
    {
        private object _obj0;
        private object _obj1;
        private object _obj2;


        public object Obj0 => _obj0 ?? null;
        public object Obj1
        {
            get => _obj1 ?? null;
        }

        public object Obj2
        {
            get
            {
                return _obj2 ?? null;
            }
        }

        public string Name { get; set; }
    }

    [AutoEqualsAndHashCode]
    public abstract partial class DataBase
    {
        public string Name { get; set; }
    }
}
