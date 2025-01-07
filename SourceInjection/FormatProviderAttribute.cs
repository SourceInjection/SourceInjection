using System;

namespace SourceInjection
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class FormatProviderAttribute : Attribute
    {
        public FormatProviderAttribute(Type formatProvider)
            : this (formatProvider?.FullName) 
        { }

        public FormatProviderAttribute(Type staticClass, string factoryMember)
            : this(staticClass?.FullName, factoryMember) 
        { }

        private FormatProviderAttribute(string formatProvider)
        {
            Class = formatProvider;
        }

        private FormatProviderAttribute(string staticClass, string factoryMember)
        {
            Class = staticClass;
            Member = factoryMember;
        }

        public string Class { get; }

        public string Member { get; }
    }
}
