using Aspects.Attributes;


namespace Aspects.Test.Common
{
    
    public partial class Data
    {
        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private string _stringValue;

        [NotifyPropertyChanging]
        [NotifyPropertyChanged]
        private object _oValue;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private IEnumerable<object> _values;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private MyStruct _str;

        [NotifyPropertyChanged(true)]
        private int _intValue;


        [NotifyPropertyChanging(true)]
        private int _intValue2;

        [NotifyPropertyChanged()]
        private int _intValue3;


        [NotifyPropertyChanging()]
        private int _intValue4;
    }
}
