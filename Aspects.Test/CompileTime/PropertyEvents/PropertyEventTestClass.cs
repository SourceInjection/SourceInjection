using Aspects.Attributes;

namespace Aspects.Test.CompileTime.PropertyEvents
{

#pragma warning disable IDE0044, IDE0051

    public partial class PropertyEventTestClass
    {
        public enum En { x, y, z }

        [NotifyPropertyChanging()]
        private string _stringChangingNoEquality = null!;

        [NotifyPropertyChanging(true)]
        private string _stringChanging = null!;

        [NotifyPropertyChanged()]
        private string? _stringChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private string? _stringChanged;

        [NotifyPropertyEvents()]
        private string _stringBothNoEquality = null!;

        [NotifyPropertyEvents(true)]
        private string _stringBoth = null!;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private string _stringBothChangingEquality = null!;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private string _stringBothChangedEquality = null!;

        [NotifyPropertyChanging()]
        private object _objectChangingNoEquality = null!;

        [NotifyPropertyChanging(true)]
        private object _objectChanging = null!;

        [NotifyPropertyChanged()]
        private object? _objectChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private object? _objectChanged;

        [NotifyPropertyEvents()]
        private object _objectBothNoEquality = null!;

        [NotifyPropertyEvents(true)]
        private object _objectBoth = null!;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private object _objectBothChangingEquality = null!;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private object _objectBothChangedEquality = null!;

        [NotifyPropertyChanging()]
        private En _EnChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private En _EnChanging;

        [NotifyPropertyChanged()]
        private En? _EnChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private En? _EnChanged;

        [NotifyPropertyEvents()]
        private En _EnBothNoEquality;

        [NotifyPropertyEvents(true)]
        private En _EnBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private En _EnBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private En _EnBothChangedEquality;

        [NotifyPropertyChanging()]
        private IEnumerable<string> _collectionChangingNoEquality = null!;

        [NotifyPropertyChanging(true)]
        private IEnumerable<string> _collectionChanging = null!;

        [NotifyPropertyChanged()]
        private IEnumerable<string>? _collectionChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private IEnumerable<string>? _collectionChanged;

        [NotifyPropertyEvents()]
        private IEnumerable<string> _collectionBothNoEquality = null!;

        [NotifyPropertyEvents(true)]
        private IEnumerable<string> _collectionBoth = null!;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private IEnumerable<string> _collectionBothChangingEquality = null!;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private IEnumerable<string> _collectionBothChangedEquality = null!;

        [NotifyPropertyChanging()]
        private int _intChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private int _intChanging;

        [NotifyPropertyChanged()]
        private int? _intChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private int? _intChanged;

        [NotifyPropertyEvents()]
        private int _intBothNoEquality;

        [NotifyPropertyEvents(true)]
        private int _intBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private int _intBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private int _intBothChangedEquality;
    }
}

#pragma warning restore IDE0044, IDE0051
