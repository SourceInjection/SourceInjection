
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Aspects.Attributes.Base
{
    public class BasicOverrideMethodAttribute : Attribute
    {
        protected BasicOverrideMethodAttribute(DataMemberKind dataMemberKind, params string[] excludedMembers) 
        {
            DataMemberKind = dataMemberKind;
            ExcludedMembers = excludedMembers.ToImmutableHashSet();
        }

        public DataMemberKind DataMemberKind { get; }

        public IImmutableSet<string> ExcludedMembers { get; }

        internal static BasicOverrideMethodAttribute FromAttributeData(AttributeData data)
        {
            var kind = (DataMemberKind)data.ConstructorArguments[0].Value;

            if (data.ConstructorArguments.Length == 1)
                return new BasicOverrideMethodAttribute(kind);

            var excluded = data.ConstructorArguments[1].Values
                .Select(obj => (string)obj.Value)
                .ToArray();

            return new BasicOverrideMethodAttribute(kind, excluded);
        }
    }
}
