using Aspects.Common.Paths;

namespace Aspects.SourceGenerators.Common
{
    internal class HashCodeCodeInfo
    {
        private readonly string _name;

        public HashCodeCodeInfo(string name)
        {
            _name = name;
        }

        public string ComparerHashCode(string comparer, bool nullSafe) 
            => HashCode($"new {comparer}().GetHashCode({_name})", nullSafe);

        public string CombinedHashCode(bool nullSafe) 
            => HashCode($"{NameOf.AspectsCombinedHashCode}({_name})", nullSafe);

        public string DeepCombinedHashCode(bool nullSafe) 
            => HashCode($"{NameOf.AspectsDeepCombinedHashCode}({_name})", nullSafe);

        internal string ComparerNullableNonReferenceTypeHashCode(string comparer, bool nullSafe)
            => HashCode($"new {comparer}().GetHashCode({_name}.Value)", nullSafe);

        private string HashCode(string hashCodeCode, bool nullSafe)
        {
            return nullSafe
                ? $"{_name} == null ? 0 : {hashCodeCode}"
                : hashCodeCode;
        }
    }
}
