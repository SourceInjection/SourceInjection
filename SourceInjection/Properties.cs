using SourceInjection.Interfaces;
using Microsoft.CodeAnalysis;
using System;
using System.ComponentModel;
using System.Collections.Generic;

namespace SourceInjection
{
    /// <summary>
    /// Abstract attribute base for <see cref="INotifyPropertyChanged.PropertyChanged"/> and / or <see cref="INotifyPropertyChanging.PropertyChanging"/> code generation.
    /// </summary>
    public abstract class PropertyEventGenerationAttribute : Attribute, IPropertyEventGenerationAttribute
    {
        protected PropertyEventGenerationAttribute(
            bool inEqualityCheck,
            NullSafety inEqualityNullSafety,
            AccessModifier setterModifier,
            params string[] relatedProperties)
        {
            InEqualityCheck = inEqualityCheck;
            NullSafety = inEqualityNullSafety;
            SetterModifier = setterModifier;
            RelatedProperties = relatedProperties;
        }

        public bool InEqualityCheck { get; }

        public NullSafety NullSafety { get; }

        public IEnumerable<string> RelatedProperties { get; }

        public AccessModifier SetterModifier {  get; }

        public AccessModifier Modifier => AccessModifier.Public;

        public AccessModifier GetterModifier => AccessModifier.None;

        public string PropertyName(IFieldSymbol field)
        {
            var fieldName = field.Name;

            while (fieldName.Length > 0 && fieldName[0] == '_')
                fieldName = fieldName.Substring(1);

            if (fieldName.Length > 0 && char.IsLetter(fieldName[0]) && char.IsLower(fieldName[0]))
                fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            return fieldName;
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
        public NotifyPropertyChangedAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                AccessModifier setterModifier = AccessModifier.None,
                params string[] relatedProperties)

            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  setterModifier,
                  relatedProperties)
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
        public NotifyPropertyChangingAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                AccessModifier setterModifier = AccessModifier.None,
                params string[] relatedProperties)

            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  setterModifier,
                  relatedProperties)
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
        public NotifyPropertyEventsAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                AccessModifier setterModifier = AccessModifier.None,
                params string[] relatedProperties)

            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  setterModifier,
                  relatedProperties)
        { }
    }
}
