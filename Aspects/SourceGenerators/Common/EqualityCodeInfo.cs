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
            return nullSafe
                ? NullSafeMethodEquality()
                : MayInversed($"{_nameA}.{nameof(Equals)}({_nameB})");
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

        private string NullSafeMethodEquality()
        {
            return _isInequality
                ? $"{_nameA} != null && !{_nameA}.{nameof(Equals)}({_nameB}) || {_nameA} == null && {_nameB} != null"
                : $"{_nameA} == null && {_nameB} == null || {_nameA}?.{nameof(Equals)}({_nameB}) == true";
        }

        private string MayInversed(string s) => _isInequality ? '!' + s : s;

        private string NullSafeSequenceEquality(string method)
        {
            return _isInequality
                ? $"!({_nameA} == {_nameB}) || {_nameA} != null && {_nameB} != null && !{method}({_nameA}, {_nameB})"
                : $"{_nameA} == {_nameB} || {_nameA} != null && {_nameB} != null && {method}({_nameA}, {_nameB})";
        }

        private string SequenceEquality(string method) => MayInversed($"{method}({_nameA}, {_nameB})");
    }
}
