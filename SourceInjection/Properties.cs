using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration;
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
            bool equalityCheck = true, 
            NullSafety inEqualityNullSafety = NullSafety.Auto, 
            string equalityComparer = null, 
            bool throwIfValueIsNull = false, 
            params string[] relatedProperties)
        {
            EqualityCheck = equalityCheck;
            NullSafety = inEqualityNullSafety;
            EqualityComparer = equalityComparer;
            ThrowIfValueIsNull = throwIfValueIsNull;
            RelatedProperties = relatedProperties;
        }

        public bool EqualityCheck { get; }

        public string EqualityComparer { get; }

        public NullSafety NullSafety { get; }

        public bool ThrowIfValueIsNull { get; }

        public IEnumerable<string> RelatedProperties { get; }

        public Microsoft.CodeAnalysis.Accessibility Accessibility => Microsoft.CodeAnalysis.Accessibility.Public;

        public Microsoft.CodeAnalysis.Accessibility GetterAccessibility => Microsoft.CodeAnalysis.Accessibility.NotApplicable;

        public string PropertyName(IFieldSymbol field)
        {
            return Snippets.PropertyNameFromField(field);
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
                Type equalityComparer = null,
                bool throwIfValueIsNull = false/*,
                params string[] relatedProperties*/)

            : this(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer?.FullName,
                  throwIfValueIsNull/*,
                  relatedProperties*/)
        { }

        protected NotifyPropertyChangedAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                string equalityComparer = null,
                bool throwIfValueIsNull = false /*,
                params string[] relatedProperties*/)

            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer,
                  throwIfValueIsNull,
                  /*relatedProperties*/Array.Empty<string>())
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
                Type equalityComparer = null,
                bool throwIfValueIsNull = false,
                params string[] relatedProperties)

            : this(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer?.FullName,
                  throwIfValueIsNull,
                  relatedProperties)
        { }

        protected NotifyPropertyChangingAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                string equalityComparer = null,
                bool throwIfValueIsNull = false,
                params string[] relatedProperties)
            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer,
                  throwIfValueIsNull,
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
                Type equalityComparer = null,
                bool throwIfValueIsNull = false,
                params string[] relatedProperties)

            : this(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer?.FullName,
                  throwIfValueIsNull,
                  relatedProperties)
        { }

        protected NotifyPropertyEventsAttribute(
                bool equalityCheck = true,
                NullSafety inEqualityNullSafety = NullSafety.Auto,
                string equalityComparer = null,
                bool throwIfValueIsNull = false,
                params string[] relatedProperties)
            : base(
                  equalityCheck,
                  inEqualityNullSafety,
                  equalityComparer,
                  throwIfValueIsNull,
                  relatedProperties)
        { }
    }
}
