using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute 
    {
        public NotifyPropertyChangedAttribute(Accessibility setterVisibility = Accessibility.Public, bool equalityCheck = false)
        {
            Visibility = setterVisibility;
            EqualityCheck = equalityCheck;
        }

        public Accessibility Visibility { get; }

        public bool EqualityCheck { get; }

        internal static NotifyPropertyChangedAttribute FromAttributeData(AttributeData data)
        {
            return new NotifyPropertyChangedAttribute(
                (Accessibility)data.ConstructorArguments[0].Value,
                (bool)data.ConstructorArguments[1].Value);
        }
    }
}
