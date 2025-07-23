using SourceInjection.Interfaces;
using SourceInjection.SourceGeneration.Base;
using SourceInjection.CodeAnalysis;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TypeInfo = SourceInjection.SourceGeneration.Common.TypeInfo;
using SourceInjection.SourceGeneration.DataMembers;
using SourceInjection.SourceGeneration;
using SourceInjection.SourceGeneration.Common;
using SourceInjection.Util;

#pragma warning disable IDE0130

namespace SourceInjection
{
    [Generator(LanguageNames.CSharp)]
    internal class SGProperty : TypeSourceGeneratorBase
    {
        private const string PropertyChanged = nameof(INotifyPropertyChanged.PropertyChanged);
        private const string PropertyChangedArgs = nameof(PropertyChangedEventArgs);
        private const string PropertyChangedHandler = nameof(PropertyChangedEventHandler);
        private const string PropertyChangedNotifyMethod = "RaisePropertyChanged";
        private static readonly string ChangedRaiseMethod =
$@"protected virtual void {PropertyChangedNotifyMethod}(string propertyName)
{{
    {PropertyChanged}?.Invoke(this, new {PropertyChangedArgs}(propertyName));
}}";

        private const string PropertyChanging = nameof(INotifyPropertyChanging.PropertyChanging);
        private const string PropertyChangingArgs = nameof(PropertyChangingEventArgs);
        private const string PropertyChangingHandler = nameof(PropertyChangingEventHandler);
        private const string PropertyChangingNotifyMethod = "RaisePropertyChanging";
        private static readonly string ChangingRaiseMethod =
$@"protected virtual void {PropertyChangingNotifyMethod}(string propertyName)
{{
    {PropertyChanging}?.Invoke(this, new {PropertyChangingArgs}(propertyName));
}}";

        protected internal override string Name { get; } = "Property";

        protected override IEnumerable<string> Dependencies(TypeInfo typeInfo)
        {
            yield return "using System.ComponentModel;";
        }

        protected override bool IsTargeted(INamedTypeSymbol symbol)
        {
            return symbol.GetMembers()
                .Any(m => m.HasAttributeOfType<INotifyPropertyChangingAttribute>() || m.HasAttributeOfType<INotifyPropertyChangedAttribute>());
        }

        protected override IEnumerable<string> InterfacesToAdd(TypeInfo typeInfo)
        {
            if (CanBeImplemented<INotifyPropertyChangingAttribute, INotifyPropertyChanging>(typeInfo))
                yield return nameof(INotifyPropertyChanging);

            if (CanBeImplemented<INotifyPropertyChangedAttribute, INotifyPropertyChanged>(typeInfo))
                yield return nameof(INotifyPropertyChanged);
        }

        protected override string TypeBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();
            var fields = GetFields(typeInfo);

            AddHandlers(sb, typeInfo);

            AddPropertyCode(sb, fields.First(), typeInfo.HasNullableEnabled);
            foreach (var field in fields.Skip(1))
                AddPropertyCode(sb.AppendLines(2), field, typeInfo.HasNullableEnabled);

            AddRaiseMethods(sb, typeInfo);

            return sb.ToString();
        }

        private void AddHandlers(StringBuilder sb, TypeInfo typeInfo)
        {
            if (MustAddHandler<INotifyPropertyChangingAttribute>(typeInfo, PropertyChanging))
                sb.AppendLine($"public event {PropertyChangingHandler} {PropertyChanging};").AppendLine();

            if (MustAddHandler<INotifyPropertyChangedAttribute>(typeInfo, PropertyChanged))
                sb.AppendLine($"public event {PropertyChangedHandler} {PropertyChanged};").AppendLine();
        }

        private void AddRaiseMethods(StringBuilder sb, TypeInfo typeInfo)
        {
            if (MustAddRaiseMethod<INotifyPropertyChangingAttribute>(typeInfo, PropertyChangingNotifyMethod))
                sb.AppendLines(2).Append(ChangingRaiseMethod);

            if (MustAddRaiseMethod<INotifyPropertyChangedAttribute>(typeInfo, PropertyChangedNotifyMethod))
                sb.AppendLines(2).Append(ChangedRaiseMethod);
        }


        private static void AddPropertyCode(StringBuilder sb, IFieldSymbol field, bool nullableEnabled)
        {
            var changingAttribute = GetAttribute<INotifyPropertyChangingAttribute>(field);
            var changedAttribute = GetAttribute<INotifyPropertyChangedAttribute>(field);

            var propertyName = changingAttribute?.PropertyName(field) 
                ?? changedAttribute?.PropertyName(field);

            var isNullableField = field.Type.NullableAnnotation == NullableAnnotation.Annotated;

            var fi = new PropertyEventFieldInfo(
                propertyName, field,
                changingAttribute, changedAttribute,
                !nullableEnabled || field.Type.HasNullableAnnotation());

            if (isNullableField)
                sb.AppendLine("#nullable enable");

            sb.AppendLine($"public {field.Type.ToDisplayString()} {propertyName}");
            sb.AppendLine("{");
            sb.AppendLine(GetterCode(field));
            sb.AppendLine(SetterCode(fi));
            sb.Append("}");

            if (isNullableField)
                sb.AppendLine().Append("#nullable restore");
        }

        private static string GetterCode(IFieldSymbol field)
        {
            return Text.Indent($"get => {field.Name};");
        }

        private static string SetterCode(PropertyEventFieldInfo fieldInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Text.Indent($"{SetterAccessibilityText(fieldInfo)}set"));
            sb.AppendLine(Text.Indent("{"));

            if(fieldInfo.ThrowIfNull)
            {
                sb.AppendLine(Text.Indent("if(value == null)", 2));
                sb.AppendLine(Text.Indent("throw new System.ArgumentNullException(\"value\"); ", 3));
            }

            if (fieldInfo.ChangingAttribute is null)
                sb.AppendLine(SetterCodeWithoutChangingEvent(fieldInfo));
            else if (!fieldInfo.ChangingAttribute.InEqualityCheck)
                sb.AppendLine(SetterCodeWithoutChangingEqualityCheck(fieldInfo));
            else
                sb.AppendLine(SetterCodeWithChangingEqualityCheck(fieldInfo));

            sb.Append(Text.Indent("}"));
            return sb.ToString();
        }

        private static string SetterAccessibilityText(PropertyEventFieldInfo fieldInfo)
        {
            var setterAccessibility = MergeAccessibilities(fieldInfo.ChangingAttribute?.SetterModifier, fieldInfo.ChangedAttribute?.SetterModifier);
            if (setterAccessibility == AccessModifier.Public)
                setterAccessibility = AccessModifier.None;
            return AccessibilityText(setterAccessibility);
        }

        private static string AccessibilityText(AccessModifier accessibility)
        {
            switch (accessibility)
            {
                case AccessModifier.Public: return "public ";
                case AccessModifier.Internal: return "internal ";
                case AccessModifier.Protected: return "protected ";
                case AccessModifier.ProtectedInternal: return "protected internal ";
                case AccessModifier.ProtectedPrivate: return "protected private ";
                case AccessModifier.Private: return "private ";
                default: return string.Empty;
            }
        }

        private static AccessModifier MergeAccessibilities(AccessModifier? a, AccessModifier? b)
        {
            if (a == null && b == null)
                return AccessModifier.None;

            if (a != null && b == null)
                return a.Value;

            if (a == null)
                return b.Value;

            if (a.Value == AccessModifier.Public || b.Value == AccessModifier.Public)
                return AccessModifier.Public;

            if (a.Value == AccessModifier.ProtectedInternal || b.Value == AccessModifier.ProtectedInternal || a.Value == AccessModifier.Internal && b.Value == AccessModifier.Protected || b.Value == AccessModifier.Internal && a.Value == AccessModifier.Protected)
                return AccessModifier.ProtectedInternal;

            if (a.Value == AccessModifier.Internal || b.Value == AccessModifier.Internal)
                return AccessModifier.Internal;

            if (a.Value == AccessModifier.Protected || b.Value == AccessModifier.Protected)
                return AccessModifier.Protected;

            if(a.Value == AccessModifier.ProtectedPrivate || b.Value == AccessModifier.ProtectedPrivate)
                return AccessModifier.ProtectedPrivate;

            if(a.Value == AccessModifier.Private || b.Value == AccessModifier.Private)
                return AccessModifier.Private;

            return AccessModifier.None;
        }
        

        private static string SetterCodeWithoutChangingEvent(PropertyEventFieldInfo fieldInfo)
        {
            var sb = new StringBuilder();

            if (fieldInfo.ChangedAttribute?.InEqualityCheck is true)
            {
                const int tab = 3;
                sb.AppendLine(InequalityConditionCode(fieldInfo.Field, fieldInfo.NullSafe));
                sb.AppendLine(Text.Indent("{", tab - 1));
                sb.AppendLine(SetField(fieldInfo.Field.Name, tab));
                AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab);
                sb.AppendLine();
                sb.Append(Text.Indent("}", tab - 1));
            }
            else
            {
                const int tab = 2;
                sb.AppendLine(SetField(fieldInfo.Field.Name, tab));
                AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab);
            }
            return sb.ToString();
        }

        private static string SetterCodeWithoutChangingEqualityCheck(PropertyEventFieldInfo fieldInfo)
        {
            const int tab = 2;

            string tempVar = null;
            var sb = new StringBuilder();

            AppendRaiseChangingEvent(sb, fieldInfo.ChangingAttribute, fieldInfo.PropertyName, tab);
            sb.AppendLine();

            if (fieldInfo.ChangedAttribute?.InEqualityCheck is true)
            {
                tempVar = Snippets.UnconflictingVariable(fieldInfo.Field.ContainingType);
                sb.AppendLine(Text.Indent($"var {tempVar} = {fieldInfo.Field.Name};", tab));
            }

            sb.Append(SetField(fieldInfo.Field.Name, tab));

            if (fieldInfo.ChangedAttribute != null)
            {
                sb.AppendLine();
                if (!fieldInfo.ChangedAttribute.InEqualityCheck)
                    AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab);
                else
                {
                    sb.AppendLine(InequalityConditionCode(fieldInfo.Field, fieldInfo.NullSafe, $"{tempVar}"));
                    sb.AppendLine(Text.Indent("{", tab));
                    AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab + 1);
                    sb.AppendLine().Append(Text.Indent("}", tab));
                }
            }
            return sb.ToString();
        }

        private static string SetterCodeWithChangingEqualityCheck(PropertyEventFieldInfo fieldInfo)
        {
            var sb = new StringBuilder();

            const int tab = 3;
            sb.AppendLine(InequalityConditionCode(fieldInfo.Field, fieldInfo.NullSafe));
            sb.AppendLine(Text.Indent("{", tab - 1));
            AppendRaiseChangingEvent(sb, fieldInfo.ChangingAttribute, fieldInfo.PropertyName, tab);

            if (!(fieldInfo.ChangedAttribute?.InEqualityCheck is false))
                sb.AppendLine(SetField(fieldInfo.Field.Name, tab));

            if (fieldInfo.ChangedAttribute?.InEqualityCheck is true)
                AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab);

            sb.AppendLine(Text.Indent("}", tab - 1));
            if (fieldInfo.ChangedAttribute?.InEqualityCheck is false)
            {
                sb.AppendLine(SetField(fieldInfo.Field.Name, tab - 1));
                AppendRaiseChangedEvent(sb, fieldInfo.ChangedAttribute, fieldInfo.PropertyName, tab - 1);
            }
            return sb.ToString().TrimEnd();
        }

        private static string InequalityConditionCode(IFieldSymbol symbol, bool nullSafe, string other = "value")
        {
            var comparerInfo = GetComparerInfo(symbol);

            var member = FieldSymbolInfo.Create(symbol);
            return Text.Indent(
                $"if ({Snippets.InequalityCheck(member, other, nullSafe, comparerInfo)})", 2);
        }

        private static EqualityComparerInfo GetComparerInfo(IFieldSymbol symbol)
        {
            var config = GetComparerConfig(symbol);
            if (string.IsNullOrEmpty(config?.EqualityComparer))
                return null;

            return EqualityComparerInfo.Get(config.EqualityComparer, symbol.Type);
        }

        private static EqualityComparerAttribute GetComparerConfig(IFieldSymbol symbol)
        {
            var attData = symbol.AttributesOfType<EqualityComparerAttribute>()
                .FirstOrDefault();

            if(attData != null && AttributeFactory.TryCreate<EqualityComparerAttribute>(attData, out var config))
                return config;

            return null;
        }

        private static T GetAttribute<T>(IFieldSymbol field) where T : class
        {
            var attData = field.AttributesOfType<T>()
                .FirstOrDefault();

            if (attData != null && AttributeFactory.TryCreate<T>(attData, out var att))
                return att;
            return null;
        }

        private static string SetField(string fieldName, int tabCount)
            => Text.Indent($"{fieldName} = value;", tabCount);

        private static string ChangingEventCall(string propName, int tabCount)
            => Text.Indent($"{PropertyChangingNotifyMethod}(\"{propName}\");", tabCount);

        private static string ChangedEventCall(string propName, int tabCount)
            => Text.Indent($"{PropertyChangedNotifyMethod}(\"{propName}\");", tabCount);

        private static void AppendRaiseChangingEvent(StringBuilder sb, INotifyPropertyChangingAttribute attribute, string propName, int tabCount)
        {
            sb.Append(ChangingEventCall(propName, tabCount));
            if (!attribute.RelatedProperties.Any())
                return;
            
            foreach (var prop in attribute.RelatedProperties)
                sb.AppendLine().Append(ChangingEventCall(prop, tabCount));            
        }

        private static void AppendRaiseChangedEvent(StringBuilder sb, INotifyPropertyChangedAttribute attribute, string propName, int tabCount)
        {
            sb.Append(ChangedEventCall(propName, tabCount));
            if (!attribute.RelatedProperties.Any())
                return;
            
            foreach (var prop in attribute.RelatedProperties)
                sb.AppendLine().Append(ChangedEventCall(prop, tabCount));
        }

        private IEnumerable<IFieldSymbol> GetFields(TypeInfo typeInfo)
        {
            return typeInfo.Symbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.HasAttributeOfType<INotifyPropertyChangedAttribute>()
                    || f.HasAttributeOfType<INotifyPropertyChangingAttribute>());
        }

        private bool CanBeImplemented<TAttribute, TInterface>(TypeInfo typeInfo)
        {
            return GetFields(typeInfo).Any(f => f.HasAttributeOfType<TAttribute>())
                && !typeInfo.Symbol.Implements<TInterface>();
        }

        private bool MustAddHandler<TAttribute>(TypeInfo typeInfo, string memberName)
        {
            return GetFields(typeInfo).Any(f => f.HasAttributeOfType<TAttribute>())
                && !typeInfo.Members(true).OfType<IEventSymbol>().Any(sy => sy.Name == memberName);
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
                    || method.Parameters.Length > 1 && method.Parameters.Skip(1).All(p => p.HasExplicitDefaultValue || p.IsParams))
                && method.Parameters[0].Type.IsString();
        }
    }
}

#pragma warning restore IDE0130