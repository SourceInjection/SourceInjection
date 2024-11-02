#pragma warning disable

namespace SourceInjection.Test.ToString.Accessibility
{
    internal class AccessibilityResources
    {
        internal static IEnumerable<string> GetTargetedFields(Accessibilities accessibility)
        {
            // do not change order !!!
            if (accessibility.HasFlag(Accessibilities.Public)) yield return "_publicField";
            if (accessibility.HasFlag(Accessibilities.ProtectedInternal)) yield return "_protectedInternalField";
            if (accessibility.HasFlag(Accessibilities.Internal)) yield return "_internalField";
            if (accessibility.HasFlag(Accessibilities.Protected)) yield return "_protectedField";
            if (accessibility.HasFlag(Accessibilities.ProtectedPrivate)) yield return "_protectedPrivateField";
            if (accessibility.HasFlag(Accessibilities.Private)) yield return "_privateField";
        }

        public static readonly object[] TestConfigs = 
        [
            new object [] { typeof(ClassWithAccessibilityNone), Accessibilities.None },
            new object [] { typeof(ClassWithDefaultAttribute), Accessibilities.Public },
            new object [] { typeof(ClassWithAccessibilityPublic), Accessibilities.Public },
            new object [] { typeof(ClassWithAccessibilityInternal), Accessibilities.Internal },
            new object [] { typeof(ClassWithAccessibilityProtected), Accessibilities.Protected },
            new object [] { typeof(ClassWithAccessibilityPrivate), Accessibilities.Private },
            new object [] { typeof(ClassWithAccessibilityProtectedInternal), Accessibilities.ProtectedInternal },
            new object [] { typeof(ClassWithAccessibilityProtectedPrivate), Accessibilities.ProtectedPrivate },
            new object [] { typeof(ClassWithAccessibilityAll), Accessibilities.All },
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

    [AutoToString(accessibility: Accessibilities.Public)]
    internal partial class ClassWithAccessibilityPublic : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.Internal)]
    internal partial class ClassWithAccessibilityInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.Protected)]
    internal partial class ClassWithAccessibilityProtected : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.Private)]
    internal partial class ClassWithAccessibilityPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.ProtectedInternal)]
    internal partial class ClassWithAccessibilityProtectedInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.ProtectedPrivate)]
    internal partial class ClassWithAccessibilityProtectedPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.None)]
    internal partial class ClassWithAccessibilityNone : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: Accessibilities.All)]
    internal partial class ClassWithAccessibilityAll: ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }
}

#pragma warning restore