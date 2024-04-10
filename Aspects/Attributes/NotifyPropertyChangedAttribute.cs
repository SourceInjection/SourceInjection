using System;

namespace Aspects.Attributes
{
    public enum SetterVisibility 
    {
        Public,
        Internal, 
        Protected, 
        Private,
        ProtectedInternal, 
        ProtectedPrivate,
    }


    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class NotifyPropertyChangedAttribute : Attribute 
    { 
        //public NotifyPropertyChangedAttribute(SetterVisibility setterVisibility = SetterVisibility.Public, bool equalityCheck = false)
        //{
        //    Visibility = setterVisibility;
        //    EqualityCheck = equalityCheck;
        //}

        public SetterVisibility Visibility { get; }

        public bool EqualityCheck { get; }
    }
}
