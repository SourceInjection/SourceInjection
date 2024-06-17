namespace Aspects.Parsers.CSharp
{
    internal class UsingTupleDefinition(string value, TupleDefinition tuple) 
        : UsingDirectiveDefinition(value)
    {
        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.TupleDefinition;

        public TupleDefinition Tuple => tuple;
    }
}
