using SourceInjection.Interfaces;
using System;

namespace SourceInjection
{
    public class EqualityComparerAttribute : Attribute, IEqualityComparerAttribute
    {
        public EqualityComparerAttribute(Type equalityComparer, NullSafety nullSafety = NullSafety.Auto)
            : this(equalityComparer?.FullName, nullSafety)
        { }

        protected EqualityComparerAttribute(string equalityComparer, NullSafety nullSafety = NullSafety.Auto)
        {
            EqualityComparer = equalityComparer;
            NullSafety = nullSafety;
        }

        public string EqualityComparer { get; }

        public NullSafety NullSafety { get; }
    }
}
