using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration;
using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel;

namespace SourceInjection
{
    /// <summary>
    /// Abstract attribute base for <see cref="INotifyPropertyChanged.PropertyChanged"/> and / or <see cref="INotifyPropertyChanging.PropertyChanging"/> code generation.
    /// </summary>
    public abstract class PropertyEventGenerationAttribute : Attribute, IGeneratesDataMemberPropertyFromFieldAttribute
    {
        protected PropertyEventGenerationAttribute(bool equalityCheck = false)
        {
            EqualityCheck = equalityCheck;
        }

        public bool EqualityCheck { get; }

        public string PropertyName(IFieldSymbol field)
        {
            return Snippets.PropertyNameFromField(field);
        }

        public Microsoft.CodeAnalysis.Accessibility Accessibility => Microsoft.CodeAnalysis.Accessibility.Public;

        public Microsoft.CodeAnalysis.Accessibility GetterAccessibility => Microsoft.CodeAnalysis.Accessibility.NotApplicable;
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanged.PropertyChanged"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangedEventHandler"/> PropertyChanged and <see cref="INotifyPropertyChanged"/> to the class if not exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangedAttribute
    {
        public NotifyPropertyChangedAttribute(bool equalityCheck = false)
            : base(equalityCheck)
        { }
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanging.PropertyChanging"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangingEventHandler"/> PropertyChanging and <see cref="INotifyPropertyChanging"/> to the class if not exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangingAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangingAttribute
    {
        public NotifyPropertyChangingAttribute(bool equalityCheck = false)
            : base(equalityCheck)
        { }
    }

    /// <summary>
    /// Attribute for automatic <see cref="INotifyPropertyChanging.PropertyChanging"/> and <see cref="INotifyPropertyChanged.PropertyChanged"/> code generation.<br/>
    /// Creates public property code that is linked to the attributed field.
    /// Also adds <see cref="PropertyChangingEventHandler"/> PropertyChanging and <see cref="PropertyChangedEventHandler"/> PropertyChanged to the class if not exist.
    /// Interfaces <see cref="INotifyPropertyChanging"/> and <see cref="INotifyPropertyChanged"/> are added if not defined.
    /// </summary>
    public class NotifyPropertyEventsAttribute : PropertyEventGenerationAttribute, INotifyPropertyChangedAttribute, INotifyPropertyChangingAttribute
    {
        public NotifyPropertyEventsAttribute(bool equalityCheck = false)
            : base(equalityCheck)
        { }
    }
}
