namespace SourceInjection.Test.PropertyEvents.PropertyChanging.Accessibility
{
    internal static class AccessibilityResources
    {
        public static readonly object[] ExpectedAccessibilities =
        [
            new object[] { typeof(ClassWithPropertyWithDefaultAttribute), AccessModifier.None },
            new object[] { typeof(ClassWithPropertyWithAccessibilityInternal), AccessModifier.Internal },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtected), AccessModifier.Protected },
            new object[] { typeof(ClassWithPropertyWithAccessibilityPrivate), AccessModifier.Private },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtectedInternal), AccessModifier.ProtectedInternal },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtectedPrivate), AccessModifier.ProtectedPrivate },
        ];
    }

    internal partial class ClassWithPropertyWithDefaultAttribute
    {
        [NotifyPropertyChanging]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPublic
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.Public)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityInternal
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.Internal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtected
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.Protected)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPrivate
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.Private)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedInternal
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.ProtectedInternal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedPrivate
    {
        [NotifyPropertyChanging(setterModifier: AccessModifier.ProtectedPrivate)]
        private int _property;
    }
}
