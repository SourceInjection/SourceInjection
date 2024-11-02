using Microsoft.CodeAnalysis;
using SourceInjection.Interfaces;

namespace SourceInjection.SourceGeneration.Common
{
    internal class PropertyEventFieldInfo
    {
        public PropertyEventFieldInfo(string propertyName,
            IFieldSymbol field,
            INotifyPropertyChangingAttribute changingAttribute,
            INotifyPropertyChangedAttribute changedAttribute,
            bool nullSafe)
        {
            PropertyName = propertyName;
            Field = field;
            ChangingAttribute = changingAttribute;
            ChangedAttribute = changedAttribute;
            NullSafe = nullSafe;
            ThrowIfNull = field.Type.IsReferenceType
                && (changingAttribute?.ThrowIfValueIsNull is true || changedAttribute?.ThrowIfValueIsNull is true);
        }

        public string PropertyName { get; }

        public IFieldSymbol Field { get; }

        public INotifyPropertyChangingAttribute ChangingAttribute { get; }

        public INotifyPropertyChangedAttribute ChangedAttribute { get; }

        public bool NullSafe { get; }

        public bool ThrowIfNull { get; }
    }
}
