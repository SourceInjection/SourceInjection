namespace SourceInjection.Test.PropertyEvents.PropertyChanged.Accessibility
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
        [NotifyPropertyChanged]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPublic
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.Public)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityInternal
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.Internal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtected
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.Protected)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPrivate
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.Private)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedInternal
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.ProtectedInternal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedPrivate
    {
        [NotifyPropertyChanged(setterModifier: AccessModifier.ProtectedPrivate)]
        private int _property;
    }
}
