namespace SourceInjection.Test.PropertyEvents.PropertyChanged.Accessibility
{
    using CompileUnits.CSharp;
    using Accessibility = Microsoft.CodeAnalysis.Accessibility;

    internal static class AccessibilityResources
    {
        public static readonly object[] ExpectedAccessibilities =
        [
            new object[] { typeof(ClassWithPropertyWithDefaultAttribute), AccessModifier.None },
            new object[] { typeof(ClassWithPropertyWithAccessibilityInternal), AccessModifier.Internal },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtected), AccessModifier.Protected },
            new object[] { typeof(ClassWithPropertyWithAccessibilityPrivate), AccessModifier.Private },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtectedInternal), AccessModifier.ProtectedInternal },
            new object[] { typeof(ClassWithPropertyWithAccessibilityProtectedPrivate), AccessModifier.PrivateProtected },
        ];
    }

    internal partial class ClassWithPropertyWithDefaultAttribute
    {
        [NotifyPropertyChanged]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPublic
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.Public)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityInternal
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.Internal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtected
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.Protected)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityPrivate
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.Private)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedInternal
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.ProtectedOrInternal)]
        private int _property;
    }

    internal partial class ClassWithPropertyWithAccessibilityProtectedPrivate
    {
        [NotifyPropertyChanged(setterAccessibility: Accessibility.ProtectedAndInternal)]
        private int _property;
    }
}
