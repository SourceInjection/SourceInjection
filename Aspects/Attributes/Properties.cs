using Aspects.Attributes.Base;
using Aspects.Attributes.Interfaces;
using System;

namespace Aspects.Attributes
{

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangedAttribute
    {
        public NotifyPropertyChangedAttribute(bool equalityCheck = false) : base (equalityCheck) { }

        public override PropertyEvent PropertyEvent { get; } = PropertyEvent.Changed;
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangingAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangingAttribute
    {
        public NotifyPropertyChangingAttribute(bool equalityCheck = false) : base(equalityCheck) { }

        public override PropertyEvent PropertyEvent { get; } = PropertyEvent.Changing;
    }
}
