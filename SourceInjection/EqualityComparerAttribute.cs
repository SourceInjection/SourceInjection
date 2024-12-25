﻿using System;

namespace SourceInjection
{
    public sealed class EqualityComparerAttribute : Attribute
    {
        public EqualityComparerAttribute(Type equalityComparer, NullSafety nullSafety = NullSafety.Auto)
            : this(equalityComparer?.FullName, nullSafety)
        { }

        private EqualityComparerAttribute(string equalityComparer, NullSafety nullSafety = NullSafety.Auto)
        {
            EqualityComparer = equalityComparer;
            NullSafety = nullSafety;
        }

        public string EqualityComparer { get; }

        public NullSafety NullSafety { get; }
    }
}
