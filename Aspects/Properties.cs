using Aspects.Interfaces;
using Aspects.SourceGeneration;
using System;
using System.ComponentModel;

namespace Aspects
{
    /// <summary>
    /// Abstract attribute base for <see cref="INotifyPropertyChanged.PropertyChanged"/> and / or <see cref="INotifyPropertyChanging.PropertyChanging"/> code generation.
    /// </summary>
    public abstract class PropertyEventGenerationAttribute : Attribute, IGeneratesPublicDataMemberPropertyFromFieldAttribute
    {
        /// <summary>
        /// Determines if a equality check is generated.
        /// </summary>
        public abstract bool EqualityCheck { get; }

        /// <summary>
        /// Evaluates the generated property name from the field name.
        /// </summary>
        /// <param name="field">The field from which the property is generated.</param>
        /// <returns>The name of the generated property.</returns>
        public string PropertyName(string fieldName)
        {
            return Snippets.PropertyNameFromField(fieldName);
        }
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanged.PropertyChanged"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangedEventHandler"/> PropertyChanged and <see cref="INotifyPropertyChanged"/> to the class if not exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangedAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="NotifyPropertyChangedAttribute"/>.
        /// </summary>
        /// <param name="equalityCheck">When set to <see langword="true"/>, event will just be fired when the value wich is set does not equal the field value.</param>
        public NotifyPropertyChangedAttribute(bool equalityCheck = false)
        {
            EqualityCheck = equalityCheck;
        }

        public override bool EqualityCheck { get; }
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanging.PropertyChanging"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangingEventHandler"/> PropertyChanging and <see cref="INotifyPropertyChanging"/> to the class if not exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangingAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangingAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="NotifyPropertyChangingAttribute"/>.
        /// </summary>
        /// <param name="equalityCheck">When set to <see langword="true"/>, event will just be fired when the value wich is set does not equal the field value.</param>
        public NotifyPropertyChangingAttribute(bool equalityCheck = false)
        {
            EqualityCheck = equalityCheck;
        }

        public override bool EqualityCheck { get; }
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanging.PropertyChanging"/> and <see cref="INotifyPropertyChanged.PropertyChanged"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangingEventHandler"/> PropertyChanging and <see cref="PropertyChangedEventHandler"/> PropertyChanged to the class if not exist.
    /// Interfaces <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/> are added if not defined.
    /// </summary>
    public class NotifyPropertyEventsAttribute : Attribute, INotifyPropertyChangedAttribute, INotifyPropertyChangingAttribute
    {
        /// <summary>
        /// Creates an instance of <see cref="NotifyPropertyEventsAttribute"/>.
        /// </summary>
        /// <param name="equalityCheck">When set to <see langword="true"/>, events will just be fired when the value wich is set does not equal the field value.</param>
        public NotifyPropertyEventsAttribute(bool equalityCheck = false)
        {
            EqualityCheck = equalityCheck;
        }

        /// <summary>
        /// Determines if a equality check occurs before <see cref="INotifyPropertyChanged.PropertyChanged"/> and <see cref="INotifyPropertyChanging.PropertyChanging"/> events are raised.
        /// </summary>
        public bool EqualityCheck { get; }

        /// <summary>
        /// Evaluates the generated property name from the field name.
        /// </summary>
        /// <param name="field">The field from which the property is generated.</param>
        /// <returns>The name of the generated property.</returns>
        public string PropertyName(string fieldName)
        {
            return Snippets.PropertyNameFromField(fieldName);
        }
    }
}
