using CompileUnits.CSharp;
using NUnit.Framework;
using System.ComponentModel;

namespace SourceInjection.Test.PropertyEvents.PropertyChanged.Inheritance
{
    [TestFixture]
    internal class InheritanceTests
    {
        private static IClass GetSut<T>()
        {
            return CodeAnalysis.CompileUnit.FromGeneratedCode<SGProperty>(typeof(T)).AllChildren()
                .OfType<IClass>()
                .Single();
        }

        private static IMethod? GetRaisedMethod<T>()
            => GetSut<T>().Methods().SingleOrDefault(IsPropertyChangedRaiseMethod);

        private static IEvent? GetHandler<T>()
            => GetSut<T>().Members.OfType<IEvent>().SingleOrDefault(IsPropertyChangedEventHandler);

        private static bool IsPropertyChangedRaiseMethod(IMethod method)
        {
            return method.Name == "RaisePropertyChanged"
                && method.Parameters.Count > 0
                && method.Parameters[0].Type.FormatedText == "string";
        }

        private static bool IsPropertyChangedEventHandler(IEvent handler)
        {
            return handler.Name == nameof(INotifyPropertyChanged.PropertyChanged)
                && handler.HasAccessibility(CompileUnits.CSharp.AccessModifier.Public);
        }

        [Test]
        public void GeneratedClass_WhenBaseHasNoHandlerAndNoRaiseMethod_GeneratesBoth()
        {
            var handler = GetHandler<Class_ThatInheritsFrom_EmptyClass>();
            var raiseMethod = GetRaisedMethod<Class_ThatInheritsFrom_EmptyClass>();

            Assert.Multiple(() =>
            {
                Assert.That(handler, Is.Not.Null);
                Assert.That(raiseMethod, Is.Not.Null);
            });
        }

        [Test]
        public void GeneratedClass_WhenBaseHasHandlerAndRaiseMethod_DoesNotGenerateAnyOfThem()
        {
            var handler = GetHandler<Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod>();
            var raiseMethod = GetRaisedMethod<Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod>();

            Assert.Multiple(() =>
            {
                Assert.That(handler, Is.Null);
                Assert.That(raiseMethod, Is.Null);
            });
        }

        [Test]
        public void GeneratedClass_WhenBaseOfBaseHasHandlerAndRaiseMethod_DoesNotGenerateAnyOfThem()
        {
            var handler = GetHandler<Class_ThatInheritsFrom_Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod>();
            var raiseMethod = GetRaisedMethod<Class_ThatInheritsFrom_Class_ThatInheritsFrom_ClassWithHandlerAndRaiseMethod>();

            Assert.Multiple(() =>
            {
                Assert.That(handler, Is.Null);
                Assert.That(raiseMethod, Is.Null);
            });
        }

        [Test]
        public void GeneratedClass_WhenSourceHasNotifyPropertyChangedInterface_DoesNotAddInterface()
        {
            var iface = GetSut<Class_WithNotifyPropertyChangedInterface>().Inheritance
                .SingleOrDefault(t => t.FormatedText == nameof(INotifyPropertyChanged));

            Assert.That(iface, Is.Null);
        }
    }
}
