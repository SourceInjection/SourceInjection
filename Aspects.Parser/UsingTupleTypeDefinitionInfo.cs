namespace Aspects.Parsers.CSharp
{
    internal class UsingTupleTypeDefinitionInfo(string value, TupleInfo tuple) 
        : UsingDirectiveInfo(value)
    {
        public override UsingDirectiveKind Kind { get; } = UsingDirectiveKind.TupleDefinition;

        public TupleInfo Tuple => tuple;
    }
}
