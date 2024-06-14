namespace Aspects.Parsers.CSharp
{
    public class ParameterInfo(string type, string name, bool isParamsArray = false, string? defaultValue = null)
    {
        public string Type => type;

        public string Name => name;

        public string? DefaultValue => defaultValue;

        public bool IsParamsArray => isParamsArray;

        public bool IsOptional { get; } = defaultValue is not null;
    }
}
