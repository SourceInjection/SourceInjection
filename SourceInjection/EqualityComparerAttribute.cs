using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    internal class EqualityComparerAttribute : Attribute, IEqualityComparerAttribute
    {
        public EqualityComparerAttribute(Type equalityComparer, NullSafety nullSafety = NullSafety.Auto)
            : this(equalityComparer?.FullName, nullSafety)
        { }

        private EqualityComparerAttribute(string equalityComparer, NullSafety nullSafety)
        {
            EqualityComparer = equalityComparer;
            NullSafety = nullSafety;
        }

        public string EqualityComparer { get; }

        public NullSafety NullSafety { get; }
    }
}
