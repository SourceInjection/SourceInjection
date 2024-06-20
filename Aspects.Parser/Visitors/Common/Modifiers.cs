namespace Aspects.Parsers.CSharp.Visitors.Common
{
    internal static class Modifiers
    {
        public static (AccessModifier? AccessModifier, bool IsStatic, bool IsSealed, bool IsAbstract, bool HasNewModifier) 
            OfClass(IEnumerable<string> modifiers)
        {
            var isStatic = false;
            var isSealed = false;
            var isAbstract = false;
            var hasNewModifier = false;
            var accessModifiers = new List<AccessModifier>();

            foreach (var modifier in modifiers )
            {
                if (modifier == "static")
                    isStatic = true;
                else if (modifier == "sealed")
                    isSealed = true;
                else if (modifier == "abstract")
                    isAbstract = true;
                else if(modifier == "new")
                    hasNewModifier = true;
                else MayAddModifier(accessModifiers, modifier);
            }
            return (MergeModifiers(accessModifiers), isStatic, isSealed, isAbstract, hasNewModifier);
        }

        public static (AccessModifier? AccessModifier, bool IsReadonly, bool HasNewModifier)
            OfStruct(IEnumerable<string> modifiers)
        {
            var isReadonly = false;
            var hasNewModifier = false;
            var accessModifiers = new List<AccessModifier>();

            foreach (var modifier in modifiers)
            {
                if (modifier == "readonly")
                    isReadonly = true;
                else if (modifier == "new")
                    hasNewModifier = true;
                else MayAddModifier(accessModifiers, modifier);
            }
            return (MergeModifiers(accessModifiers), isReadonly, hasNewModifier);
        }

        public static (AccessModifier? AccessModifier, bool HasNewModifier) OfEnum(IEnumerable<string> modifiers) => OfType(modifiers);

        public static (AccessModifier? AccessModifier, bool HasNewModifier) OfDelegate(IEnumerable<string> modifiers) => OfType(modifiers);

        public static (AccessModifier? AccessModifier, bool HasNewModifier) OfInterface(IEnumerable<string> modifiers) => OfType(modifiers);

        private static (AccessModifier? AccessModifier, bool HasNewModifier) OfType(IEnumerable<string> modifiers)
        {
            var accessModifiers = new List<AccessModifier>();
            var hasNewModifier = false;

            foreach(var modifier in modifiers)
            {
                if(modifier == "new")
                    hasNewModifier = true;
                else MayAddModifier(accessModifiers, modifier);
            }
            return (MergeModifiers(accessModifiers), hasNewModifier);
        }


        private static bool MayAddModifier(List<AccessModifier> accessModifiers, string modifier)
        {
            var cnt = accessModifiers.Count;
            if (modifier == "private")
                accessModifiers.Add(AccessModifier.Private);
            else if (modifier == "public")
                accessModifiers.Add(AccessModifier.Public);
            else if (modifier == "internal")
                accessModifiers.Add(AccessModifier.Internal);
            else if (modifier == "protected")
                accessModifiers.Add(AccessModifier.Protected);
            return cnt < accessModifiers.Count;
        }

        private static AccessModifier? MergeModifiers(List<AccessModifier> modifiers)
        {
            if(modifiers.Count == 0)
                return null;
            if (modifiers.Count == 1)
                return modifiers[0];
            if(modifiers.Count == 2 && modifiers.Contains(AccessModifier.Protected))
            {
                if (modifiers.Contains(AccessModifier.Protected))
                    return AccessModifier.ProtectedInternal;
                if (modifiers.Contains(AccessModifier.Private))
                    return AccessModifier.PrivateProtected;
            }
            throw new ArgumentException($"argument {modifiers} length can not be greater than 2 and " +
                $"if length is 2 must contain private protected or protected internal.");
        }
    }
}
