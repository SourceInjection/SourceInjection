using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.Queries;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class PropertyEventSourceGenerator : TypeSourceGeneratorBase
    {
        private const string PropertyChanged = nameof(INotifyPropertyChanged.PropertyChanged);
        private const string PropertyChangedArgs = nameof(PropertyChangedEventArgs);
        private const string PropertyChangedHandler = nameof(PropertyChangedEventHandler);

        private const string PropertyChanging = nameof(INotifyPropertyChanging.PropertyChanging);
        private const string PropertyChangingArgs = nameof(PropertyChangingEventArgs);
        private const string PropertyChangingHandler = nameof(PropertyChangingEventHandler);

        private const string PropertyChangedNotifyMethod = "RaisePropertyChanged";
        private const string PropertyChangingNotifyMethod = "RaisePropertyChanging";

        private static readonly string ChangingRaiseMethod =
$@"protected virtual void {PropertyChangingNotifyMethod}(string propertyName)
{{
    {PropertyChanging}?.Invoke(this, new {PropertyChangingArgs}(propertyName));
}}";

        private static readonly string ChangedRaiseMethod =
$@"protected virtual void {PropertyChangedNotifyMethod}(string propertyName)
{{
    {PropertyChanged}?.Invoke(this, new {PropertyChangedArgs}(propertyName));
}}";

        protected override string Name { get; } = "PropertyEvent";

        protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                    Types.WithMembersWithAttributeOfType<INotifyPropertyChangedAttribute>()
                .Or(Types.WithMembersWithAttributeOfType<INotifyPropertyChangingAttribute>()));


        protected override string Dependencies(TypeInfo typeInfo)
        {
            return "using System.ComponentModel;";
        }

        protected override IEnumerable<string> InterfacesToAdd(TypeInfo typeInfo)
        {
            if(CanBeImplemented<INotifyPropertyChangingAttribute, INotifyPropertyChanging>(typeInfo))
                yield return nameof(INotifyPropertyChanging);

            if(CanBeImplemented<INotifyPropertyChangedAttribute, INotifyPropertyChanged>(typeInfo))
                yield return nameof(INotifyPropertyChanged);
        }

        private bool CanBeImplemented<TAttribute, TInterface>(TypeInfo typeInfo)
        {
            return GetFields(typeInfo).Any(f => f.HasAttributeOfType<TAttribute>())
                && !typeInfo.Symbol.Implements<TInterface>();
        }

        private bool MustAddHandler<TAttribute>(TypeInfo typeInfo, string memberName)
        {
            return GetFields(typeInfo).Any(f => f.HasAttributeOfType<TAttribute>())
                && !typeInfo.Members(true).Any(sy => sy.Name == memberName);
        }

        private bool MustAddRaiseMethod<TAttribute>(TypeInfo typeInfo, string name)
        {
            return GetFields(typeInfo).Any(f => f.HasAttributeOfType<TAttribute>())
                && !typeInfo.Members(true)
                    .OfType<IMethodSymbol>()
                    .Any(m => MethodIsRaiseMethod(m, name));
        }

        private bool MethodIsRaiseMethod(IMethodSymbol method, string name)
        {
            return method.Name == name
                && (method.Parameters.Length == 1 
                    || method.Parameters.Length > 1 && method.Parameters.Skip(1).All(p => p.HasExplicitDefaultValue))
                && method.Parameters[0].Type.Name == nameof(String);
        }

        protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            var fields = GetFields(typeInfo);

            if (MustAddHandler<INotifyPropertyChangingAttribute>(typeInfo, PropertyChanging))
            {
                sb.AppendLine($"public event {PropertyChangingHandler} {PropertyChanging};");
                sb.AppendLine();
            }
            
            if (MustAddHandler<INotifyPropertyChangedAttribute>(typeInfo, PropertyChanged))
            {
                sb.AppendLine($"public event {PropertyChangedHandler} {PropertyChanged};");
                sb.AppendLine();
            }

            sb.Append(PropertyCode(fields.First()));
            foreach(var field in fields.Skip(1))
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(PropertyCode(field));
            }

            if (MustAddRaiseMethod<INotifyPropertyChangingAttribute>(typeInfo, PropertyChangingNotifyMethod))
            {
                sb.AppendLine();
                sb.AppendLine();

            }

            if (MustAddRaiseMethod<INotifyPropertyChangedAttribute>(typeInfo, PropertyChangedNotifyMethod))
            {
                sb.AppendLine();
                sb.AppendLine();

            }

            return sb.ToString();
        }

        private IEnumerable<IFieldSymbol> GetFields(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.HasAttributeOfType<INotifyPropertyChangedAttribute>()
                    || f.HasAttributeOfType<INotifyPropertyChangingAttribute>());
        }

        private static string PropertyCode(IFieldSymbol field)
        {
            var name = CodeSnippets.PropertyNameFromField(field);
            var sb = new StringBuilder();

            var type = field.Type.ToDisplayString();
            var enableNull = type.Length > 0 && field.Type.NullableAnnotation == NullableAnnotation.Annotated;

            if (enableNull)
                sb.AppendLine("#nullable enable");

            sb.AppendLine($"public {type} {name}");
            sb.AppendLine("{");
            sb.AppendLine(GetterCode(field));
            sb.AppendLine(SetterCode(field));
            sb.Append("}");

            if (enableNull)
            {
                sb.AppendLine();
                sb.Append("#nullable restore");
            }

            return sb.ToString();
        }

        private static string GetterCode(IFieldSymbol field)
        {
            return $"\tget => {field.Name};";
        }

        private static string SetterCode(IFieldSymbol field)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tset");

            var changingAttribute = GetAttribute<INotifyPropertyChangingAttribute>(field);
            var changedAttribute = GetAttribute<INotifyPropertyChangedAttribute>(field);

            if (changingAttribute is null)
                sb.AppendLine(SetterCodeWithoutChangingEvent(field, changedAttribute));
            else if (!changingAttribute.EqualityCheck)
                sb.Append(SetterCodeWithoutChangingEqualityCheck(field, changedAttribute));
            else
                sb.Append(SetterCodeWithChangingEqualityCheck(field, changedAttribute));

            return sb.ToString();
        }

        private static string SetterCodeWithoutChangingEvent(IFieldSymbol field, INotifyPropertyChangedAttribute changedAttribute)
        {
            var sb = new StringBuilder();
            var propertyName = CodeSnippets.PropertyNameFromField(field);

            if (changedAttribute?.EqualityCheck is true)
            {
                const int tab = 2;
                sb.AppendLine(EqualityCheckCode(field));
                sb.AppendLine("\t{");
                sb.AppendLine(SetField(field.Name, tab));
                sb.AppendLine(RaiseChangedEvent(propertyName, tab));
                sb.Append("\t}");
            }
            else
            {
                const int tab = 1;
                sb.AppendLine(SetField(field.Name, tab));
                sb.Append(RaiseChangedEvent(propertyName, tab));
            }
            return sb.ToString();
        }

        private static string SetterCodeWithoutChangingEqualityCheck(IFieldSymbol field, INotifyPropertyChangedAttribute changedAttribute)
        {
            var sb = new StringBuilder();
            var propertyName = CodeSnippets.PropertyNameFromField(field);

            const int tab = 1;
            sb.AppendLine(RaiseChangingEvent(propertyName, tab));
            sb.AppendLine(SetField(field.Name, tab));

            if (changedAttribute != null)
            {
                if (!changedAttribute.EqualityCheck)
                    sb.Append(RaiseChangedEvent(propertyName, tab));
                else
                {
                    sb.AppendLine(EqualityCheckCode(field));
                    sb.AppendLine("\t{");
                    sb.AppendLine(RaiseChangedEvent(propertyName, tab + 1));
                    sb.Append("\t}");
                }
            }
            return sb.ToString();
        }

        private static string SetterCodeWithChangingEqualityCheck(IFieldSymbol field, INotifyPropertyChangedAttribute changedAttribute)
        {
            var sb = new StringBuilder();
            var propertyName = CodeSnippets.PropertyNameFromField(field);

            const int tab = 2;
            sb.AppendLine(EqualityCheckCode(field));
            sb.AppendLine("\t{");
            sb.AppendLine(RaiseChangingEvent(propertyName, tab));

            if (!(changedAttribute?.EqualityCheck is false))
                sb.AppendLine(SetField(field.Name, tab));

            if (changedAttribute?.EqualityCheck is true)
                sb.AppendLine(RaiseChangedEvent(propertyName, tab));

            sb.AppendLine("\t}");
            if (changedAttribute?.EqualityCheck is false)
            {
                sb.AppendLine(SetField(field.Name, tab - 1));
                sb.AppendLine(RaiseChangedEvent(propertyName, tab - 1));
            }
            return sb.ToString();
        }

        private static string EqualityCheckCode(IFieldSymbol field)
        {
            if (!field.Type.IsReferenceType)
            {
                if (field.Type.IsUnmanagedType || field.Type.Name == nameof(String))
                    return $"\t\tif ({field.Name} != value)";
                return $"\t\tif (!{field.Name}.{nameof(Equals)}(value))";
            }

            if (field.Type.IsEnumerable() && !field.Type.OverridesEquals())
                return $"\t\tif (!{CodeSnippets.SequenceEqualsMethod(field.Name, "value")}";

            return $"\t\tif (!({field.Name} is null) && !{field.Name}.{nameof(Equals)}(value) " +
                $"|| {field.Name} is null && !(value is null))";
        }

        private static T GetAttribute<T>(IFieldSymbol field)
        {
            var attName = typeof(T).FullName;
            var attData = field.GetAttributes()
                .SingleOrDefault(a => a.AttributeClass.ToDisplayString() == attName);

            if (AttributeFactory.TryCreate<T>(attData, out var att))
                return att;
            return default;
        }

        private static string SetField(string fieldName, int tabCount)
        {
            return CodeSnippets.Indent($"{fieldName} = value;", tabCount);
        }

        private static string RaiseChangingEvent(string propName, int tabCount)
        {
            return CodeSnippets.Indent(
                $"{PropertyChanging}?.Invoke(this, new {PropertyChangingArgs}(nameof({propName})));", tabCount);
        }

        private static string RaiseChangedEvent(string propName, int tabCount)
        {
            return CodeSnippets.Indent(
                $"{PropertyChanged}?.Invoke(this, new {PropertyChangedArgs}(nameof({propName})));", tabCount);
        }
    }
}
