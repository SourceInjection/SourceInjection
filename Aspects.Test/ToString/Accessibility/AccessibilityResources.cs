#pragma warning disable

namespace Aspects.Test.ToString.Accessibility
{
    using Accessibility = Aspects.Accessibility;

    internal class AccessibilityResources
    {
        internal static IEnumerable<string> GetTargetedFields(Accessibility accessibility)
        {
            // do not change order !!!
            if (accessibility.HasFlag(Accessibility.Public)) yield return "_publicField";
            if (accessibility.HasFlag(Accessibility.ProtectedInternal)) yield return "_protectedInternalField";
            if (accessibility.HasFlag(Accessibility.Internal)) yield return "_internalField";
            if (accessibility.HasFlag(Accessibility.Protected)) yield return "_protectedField";
            if (accessibility.HasFlag(Accessibility.ProtectedPrivate)) yield return "_protectedPrivateField";
            if (accessibility.HasFlag(Accessibility.Private)) yield return "_privateField";
        }

        public static readonly object[] TestConfigs = 
        [
            new object [] { typeof(ClassWithAccessibilityNone), Accessibility.None },
            new object [] { typeof(ClassWithDefaultAttribute), Accessibility.Public },
            new object [] { typeof(ClassWithAccessibilityPublic), Accessibility.Public },
            new object [] { typeof(ClassWithAccessibilityInternal), Accessibility.Internal },
            new object [] { typeof(ClassWithAccessibilityProtected), Accessibility.Protected },
            new object [] { typeof(ClassWithAccessibilityPrivate), Accessibility.Private },
            new object [] { typeof(ClassWithAccessibilityProtectedInternal), Accessibility.ProtectedInternal },
            new object [] { typeof(ClassWithAccessibilityProtectedPrivate), Accessibility.ProtectedPrivate },
            new object [] { typeof(ClassWithAccessibilityAll), Accessibility.All },
        ];
    }

    internal class ClassWithAllAccessibilitiesThatCanBeInherited
    {
        // do not change order !!!
        public int _publicField;
        protected internal int _protectedInternalField;
        internal int _internalField;
        protected int _protectedField;
        protected private int _protectedPrivateField;
    }

    [AutoToString]
    internal partial class ClassWithDefaultAttribute : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.Public)]
    internal partial class ClassWithAccessibilityPublic : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.Internal)]
    internal partial class ClassWithAccessibilityInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.Protected)]
    internal partial class ClassWithAccessibilityProtected : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.Private)]
    internal partial class ClassWithAccessibilityPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.ProtectedInternal)]
    internal partial class ClassWithAccessibilityProtectedInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.ProtectedPrivate)]
    internal partial class ClassWithAccessibilityProtectedPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.None)]
    internal partial class ClassWithAccessibilityNone : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibility.All)]
    internal partial class ClassWithAccessibilityAll: ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }
}

#pragma warning restore