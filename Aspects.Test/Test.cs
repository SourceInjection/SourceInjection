
using Aspects.Attributes;
using System.ComponentModel;

namespace Aspects.Test
{
    [Serializable]
    [Browsable(false)]     
    public partial class Test<K> : INotifyPropertyChanged where K : struct, IEquatable<K>
    {
        [NotifyPropertyChanged(SetterVisibility.Protected)]
        private int _field;

        [NotifyPropertyChanged(equalityCheck: true)]
        private Herbert<K, string>? _otherField;

        [NotifyPropertyChanged(equalityCheck: true)]
        private int _anotherField;
    }
}
