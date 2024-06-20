namespace Aspects.Parsers.CSharp
{
    public class ParameterDefinition
    {
        public ParameterDefinition(string type, string name, bool isParamsArray = false, string? defaultValue = null)
        {
            Type = type;
            Name = name;
            IsParamsArray = isParamsArray;
            DefaultValue = defaultValue;
            IsOptional = defaultValue != null;
        }

        public string Type { get; }

        public string Name { get; }

        public string? DefaultValue { get; }

        public bool IsParamsArray { get; }

        public bool IsOptional { get; }
    }
}
