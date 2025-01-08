using System;

namespace SourceInjection
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FormatProviderAttribute : Attribute
    {
        public FormatProviderAttribute(Type type)
            : this (type?.FullName) 
        { }

        public FormatProviderAttribute(Type type, string property)
            : this(type?.FullName, property) 
        { }

        private FormatProviderAttribute(string type)
        {
            Type = type;
        }

        private FormatProviderAttribute(string type, string property)
        {
            Type = type;
            Property = property;
        }

        public string Type { get; }

        public string Property { get; }
    }
}
