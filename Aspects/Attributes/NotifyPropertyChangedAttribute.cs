using Microsoft.CodeAnalysis;
using System;

namespace Aspects.Attributes
{

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute 
    {
        public NotifyPropertyChangedAttribute(SetterVisibility setterVisibility = SetterVisibility.Public, bool equalityCheck = false)
        {
            Visibility = setterVisibility;
            EqualityCheck = equalityCheck;
        }

        public SetterVisibility Visibility { get; }

        public bool EqualityCheck { get; }

        internal static NotifyPropertyChangedAttribute FromAttributeData(AttributeData data)
        {
            return new NotifyPropertyChangedAttribute(
                (SetterVisibility)data.ConstructorArguments[0].Value,
                (bool)data.ConstructorArguments[1].Value);
        }
    }
}
