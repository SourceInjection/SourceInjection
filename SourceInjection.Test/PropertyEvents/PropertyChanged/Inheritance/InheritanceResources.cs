using System.ComponentModel;

namespace SourceInjection.Test.PropertyEvents.PropertyChanged.Inheritance
{
#pragma warning disable S2094 // Classes should not be empty
    internal class EmptyClass { }
#pragma warning restore S2094 // Classes should not be empty

    internal class ClassWithHandlerAndRaiseMethod
    {
        public event PropertyChangedEventHandler PropertyChanged = null!;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal partial class Class_ThatInheritsFrom_EmptyClass : EmptyClass
    {
        [NotifyPropertyChanged]
        int _property;
    }

    internal partial class Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod : ClassWithHandlerAndRaiseMethod
    {
        [NotifyPropertyChanged]
        int _property;
    }

    internal partial class Class_ThatInheritsFrom_Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod : Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod
    {
        [NotifyPropertyChanged]
        int _property;
    }

    internal partial class Class_WithNotifyPropertyChangedInterface : INotifyPropertyChanged
    {
        [NotifyPropertyChanged]
        int _property;
    }
}
