#pragma warning disable

namespace SourceInjection.Test.ToString.Accessibility
{
    internal class AccessibilityResources
    {
        internal static IEnumerable<string> GetTargetedFields(AccessibilityRestriction accessibility)
        {
            // do not change order !!!
            if (accessibility.HasFlag(AccessibilityRestriction.Public)) yield return "_publicField";
            if (accessibility.HasFlag(AccessibilityRestriction.ProtectedInternal)) yield return "_protectedInternalField";
            if (accessibility.HasFlag(AccessibilityRestriction.Internal)) yield return "_internalField";
            if (accessibility.HasFlag(AccessibilityRestriction.Protected)) yield return "_protectedField";
            if (accessibility.HasFlag(AccessibilityRestriction.ProtectedPrivate)) yield return "_protectedPrivateField";
            if (accessibility.HasFlag(AccessibilityRestriction.Private)) yield return "_privateField";
        }

        public static readonly object[] TestConfigs = 
        [
            new object [] { typeof(ClassWithAccessibilityNone), AccessibilityRestriction.None },
            new object [] { typeof(ClassWithDefaultAttribute), AccessibilityRestriction.Public },
            new object [] { typeof(ClassWithAccessibilityPublic), AccessibilityRestriction.Public },
            new object [] { typeof(ClassWithAccessibilityInternal), AccessibilityRestriction.Internal },
            new object [] { typeof(ClassWithAccessibilityProtected), AccessibilityRestriction.Protected },
            new object [] { typeof(ClassWithAccessibilityPrivate), AccessibilityRestriction.Private },
            new object [] { typeof(ClassWithAccessibilityProtectedInternal), AccessibilityRestriction.ProtectedInternal },
            new object [] { typeof(ClassWithAccessibilityProtectedPrivate), AccessibilityRestriction.ProtectedPrivate },
            new object [] { typeof(ClassWithAccessibilityAll), AccessibilityRestriction.All },
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

    [AutoToString(accessibility: AccessibilityRestriction.Public)]
    internal partial class ClassWithAccessibilityPublic : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.Internal)]
    internal partial class ClassWithAccessibilityInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.Protected)]
    internal partial class ClassWithAccessibilityProtected : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.Private)]
    internal partial class ClassWithAccessibilityPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.ProtectedInternal)]
    internal partial class ClassWithAccessibilityProtectedInternal : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.ProtectedPrivate)]
    internal partial class ClassWithAccessibilityProtectedPrivate : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.None)]
    internal partial class ClassWithAccessibilityNone : ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }

    [AutoToString(accessibility: AccessibilityRestriction.All)]
    internal partial class ClassWithAccessibilityAll: ClassWithAllAccessibilitiesThatCanBeInherited
    {
        private int _privateField;
    }
}

#pragma warning restore