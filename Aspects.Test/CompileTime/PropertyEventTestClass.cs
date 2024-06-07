using Aspects.Attributes;

namespace Aspects.Test.CompileTime
{
    public partial class PropertyEventTestClass
    {
        public enum En { x, y, z }

        [NotifyPropertyChanging()]
        private string _stringChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private string _stringChanging;

        [NotifyPropertyChanged()]
        private string? _stringChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private string? _stringChanged;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged()]
        private string _stringBothNoEquality;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private string _stringBoth;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private string _stringBothChangingEquality;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private string _stringBothChangedEquality;

        [NotifyPropertyChanging()]
        private object _objectChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private object _objectChanging;

        [NotifyPropertyChanged()]
        private object? _objectChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private object? _objectChanged;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged()]
        private object _objectBothNoEquality;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private object _objectBoth;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private object _objectBothChangingEquality;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private object _objectBothChangedEquality;

        [NotifyPropertyChanging()]
        private En _EnChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private En _EnChanging;

        [NotifyPropertyChanged()]
        private En? _EnChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private En? _EnChanged;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged()]
        private En _EnBothNoEquality;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private En _EnBoth;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private En _EnBothChangingEquality;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private En _EnBothChangedEquality;

        [NotifyPropertyChanging()]
        private IEnumerable<string> _collectionChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private IEnumerable<string> _collectionChanging;

        [NotifyPropertyChanged()]
        private IEnumerable<string>? _collectionChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private IEnumerable<string>? _collectionChanged;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged()]
        private IEnumerable<string> _collectionBothNoEquality;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private IEnumerable<string> _collectionBoth;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private IEnumerable<string> _collectionBothChangingEquality;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private IEnumerable<string> _collectionBothChangedEquality;

        [NotifyPropertyChanging()]
        private int _intChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private int _intChanging;

        [NotifyPropertyChanged()]
        private int? _intChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private int? _intChanged;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged()]
        private int _intBothNoEquality;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged(true)]
        private int _intBoth;

        [NotifyPropertyChanging(true)]
        [NotifyPropertyChanged()]
        private int _intBothChangingEquality;

        [NotifyPropertyChanging()]
        [NotifyPropertyChanged(true)]
        private int _intBothChangedEquality;
    }
}
