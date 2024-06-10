using Aspects.Attributes;
using System.Collections.Generic;

namespace Aspects.Test.NetStandard2._0.CompileTime.PropertyEvents
{

#pragma warning disable IDE0044, IDE0051

    public partial class PropertyEventTestClass
    {
        public enum En { x, y, z }

        [NotifyPropertyChanging()]
        private string _stringChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private string _stringChanging;

        [NotifyPropertyChanged()]
        private string _stringChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private string _stringChanged;

        [NotifyPropertyEvents()]
        private string _stringBothNoEquality;

        [NotifyPropertyEvents(true)]
        private string _stringBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private string _stringBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private string _stringBothChangedEquality;

        [NotifyPropertyChanging()]
        private object _objectChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private object _objectChanging;

        [NotifyPropertyChanged()]
        private object _objectChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private object _objectChanged;

        [NotifyPropertyEvents()]
        private object _objectBothNoEquality;

        [NotifyPropertyEvents(true)]
        private object _objectBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private object _objectBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private object _objectBothChangedEquality;

        [NotifyPropertyChanging()]
        private En _EnChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private En _EnChanging;

        [NotifyPropertyChanged()]
        private En _EnChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private En _EnChanged;

        [NotifyPropertyEvents()]
        private En _EnBothNoEquality;

        [NotifyPropertyEvents(true)]
        private En _EnBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private En _EnBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private En _EnBothChangedEquality;

        [NotifyPropertyChanging()]
        private IEnumerable<string> _collectionChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private IEnumerable<string> _collectionChanging;

        [NotifyPropertyChanged()]
        private IEnumerable<string> _collectionChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private IEnumerable<string> _collectionChanged;

        [NotifyPropertyEvents()]
        private IEnumerable<string> _collectionBothNoEquality;

        [NotifyPropertyEvents(true)]
        private IEnumerable<string> _collectionBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private IEnumerable<string> _collectionBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private IEnumerable<string> _collectionBothChangedEquality;

        [NotifyPropertyChanging()]
        private int _intChangingNoEquality;

        [NotifyPropertyChanging(true)]
        private int _intChanging;

        [NotifyPropertyChanged()]
        private int _intChangedNoEquality;

        [NotifyPropertyChanged(true)]
        private int _intChanged;

        [NotifyPropertyEvents()]
        private int _intBothNoEquality;

        [NotifyPropertyEvents(true)]
        private int _intBoth;

        [NotifyPropertyChanging(true), NotifyPropertyChanged()]
        private int _intBothChangingEquality;

        [NotifyPropertyChanging(), NotifyPropertyChanged(true)]
        private int _intBothChangedEquality;
    }

#pragma warning restore IDE0044, IDE0051

}