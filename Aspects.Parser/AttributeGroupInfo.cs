
namespace Aspects.Parsers.CSharp
{
    public class AttributeGroupInfo
    {
        public AttributeGroupInfo(string? attributeTarget, IReadOnlyList<AttributeInfo> attributes)
        {
            foreach (var attribute in attributes)
                attribute.ContainingSection = this;
            AttributeTarget = attributeTarget;
            Attributes = attributes;
        }

        public string? AttributeTarget { get; } 

        public IReadOnlyList<AttributeInfo> Attributes { get; }
    }
}
