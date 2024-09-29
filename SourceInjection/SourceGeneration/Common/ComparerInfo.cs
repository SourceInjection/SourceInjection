namespace SourceInjection.SourceGeneration.Common
{
    internal class ComparerInfo
    {
        public ComparerInfo(string name, bool isNullSafe)
        {
            Name = name;
            IsNullSafe = isNullSafe;
        }

        public string Name { get; }

        public bool IsNullSafe { get; }
    }
}
