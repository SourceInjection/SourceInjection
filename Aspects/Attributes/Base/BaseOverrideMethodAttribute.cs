
using System;
using System.Collections.Immutable;

namespace Aspects.Attributes.Base
{
    public abstract class BaseOverrideMethodAttribute : Attribute
    {
        protected BaseOverrideMethodAttribute(DataMemberKind dataMemberKind, params string[] excludedMembers) 
        {
            DataMemberKind = dataMemberKind;
            ExcludedMembers = excludedMembers.ToImmutableHashSet();
        }

        public DataMemberKind DataMemberKind { get; }

        public IImmutableSet<string> ExcludedMembers { get; }
    }
}
