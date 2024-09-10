using Aspects.Common.Paths;

namespace Aspects.SourceGenerators.Common
{
    internal class EqualityCodeInfo
    {
        private readonly bool _isInequality;
        private readonly string _nameA;
        private readonly string _nameB;

        public EqualityCodeInfo(string nameA, string nameB, bool isInequality = false)
        {
            _isInequality = isInequality;
            _nameA = nameA;
            _nameB = nameB;
        }

        public string LinqSequenceEquality(bool nullSafe)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.LinqSequenceEqual)
                : SequenceEquality(NameOf.LinqSequenceEqual);
        }

        public string AspectsSequenceEquality(bool nullSafe)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.AspectsDeepSequenceEqual)
                : SequenceEquality(NameOf.AspectsDeepSequenceEqual);
        }

        public string MethodEquality(bool nullSafe)
        {
            var methodCode = $"{_nameA}.{nameof(Equals)}({_nameB})";
            return nullSafe
                ? Equality(methodCode)
                : MayInversed(methodCode);
        }

        public string AspectsArrayEquality(bool nullSafe)
        {
            return nullSafe
                ? NullSafeSequenceEquality(NameOf.AspectsArraySequenceEqual)
                : SequenceEquality(NameOf.AspectsArraySequenceEqual);
        }

        public string OperatorEquality()
        {
            return _isInequality
                ? $"{_nameA} != {_nameB}"
                : $"{_nameA} == {_nameB}";
        }

        public string ComparerEquality(string comparer, bool nullSafe)
        {
            var comparerCode = $"new {comparer}().Equals({_nameA}, {_nameB})";
            return nullSafe
                ? Equality(comparerCode)
                : MayInversed(comparerCode);
        }

        public string ComparerNullableNonReferenceTypeEquality(string comparer, bool nullSafe)
        {
            var comparerCode = $"new {comparer}().Equals({_nameA}.Value, {_nameB}.Value)";
            return nullSafe
                ? Equality(comparerCode)
                : MayInversed(comparerCode);
        }

        private string MayInversed(string s) => _isInequality ? '!' + s : s;

        private string SequenceEquality(string method) => MayInversed($"{method}({_nameA}, {_nameB})");

        private string NullSafeSequenceEquality(string method)
        {
            var methodCode = $"{method}({_nameA}, {_nameB})";
            return _isInequality
                ? $"{_nameA} == null ^ {_nameB} == null || {_nameA} != null && {_nameA} != {_nameB} && !{methodCode}"
                : $"{_nameA} == {_nameB} || {_nameA} != null && {_nameB} != null && {methodCode}";
        }

        private string Equality(string methodComparison)
        {
            return _isInequality
                ? $"{_nameA} == null ^ {_nameB} == null || {_nameA} != null && !{methodComparison}"
                : $"{_nameA} == null && {_nameB} == null || {_nameA} != null && {_nameB} != null && {methodComparison}";
        }
    }
}
