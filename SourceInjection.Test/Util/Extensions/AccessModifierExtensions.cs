namespace SourceInjection.Test.Util.Extensions
{
    internal static class AccessModifierExtensions
    {
        public static CompileUnits.CSharp.AccessModifier ToCompileUnitsAccessModifier(this AccessModifier accessModifier)
        {
            if(accessModifier == AccessModifier.None)
                return CompileUnits.CSharp.AccessModifier.None;

            if (accessModifier == AccessModifier.Public)
                return CompileUnits.CSharp.AccessModifier.Public;

            if (accessModifier == AccessModifier.Private)
                return CompileUnits.CSharp.AccessModifier.Private;

            if (accessModifier == AccessModifier.Internal)
                return CompileUnits.CSharp.AccessModifier.Internal;

            if (accessModifier == AccessModifier.Protected)
                return CompileUnits.CSharp.AccessModifier.Protected;

            if (accessModifier == AccessModifier.ProtectedInternal)
                return CompileUnits.CSharp.AccessModifier.ProtectedInternal;

            if (accessModifier == AccessModifier.ProtectedPrivate)
                return CompileUnits.CSharp.AccessModifier.PrivateProtected;

            throw new NotImplementedException($"{nameof(AccessModifier)}.{accessModifier} not implemented.");
        }
    }
}
