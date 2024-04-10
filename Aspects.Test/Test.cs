
using Aspects.Attributes;
using System.ComponentModel;

namespace Aspects.Test
{
    [Serializable]
    public partial class Test<K> where K : struct, IEquatable<K>
    {
        [NotifyPropertyChanged]
        private int _field;

        [NotifyPropertyChanged]
        private Herbert<K, string> _otherField;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
