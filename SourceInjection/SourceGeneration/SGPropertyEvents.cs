﻿using SourceInjection.Interfaces;
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
    internal class SGPropertyEvents : TypeSourceGeneratorBase
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

        protected internal override string Name { get; } = "PropertyEvent";

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
                &&  method.Parameters[0].Type.IsString();
        }

        protected override string TypeBody(TypeInfo typeInfo)
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

            sb.Append(PropertyCode(fields.First(), typeInfo.HasNullableEnabled));
            foreach (var field in fields.Skip(1))
            {
                sb.AppendLines(2);
                sb.Append(PropertyCode(field, typeInfo.HasNullableEnabled));
            }

            if (MustAddRaiseMethod<INotifyPropertyChangingAttribute>(typeInfo, PropertyChangingNotifyMethod))
            {
                sb.AppendLines(2);
                sb.Append(ChangingRaiseMethod);
            }
            if (MustAddRaiseMethod<INotifyPropertyChangedAttribute>(typeInfo, PropertyChangedNotifyMethod))
            {
                sb.AppendLines(2);
                sb.Append(ChangedRaiseMethod);
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

        private static string PropertyCode(IFieldSymbol field, bool nullableEnabled)
        {
            var name = Snippets.PropertyNameFromField(field);
            var sb = new StringBuilder();

            var type = field.Type.ToDisplayString();
            var isNullableField = field.Type.NullableAnnotation == NullableAnnotation.Annotated;

            if (isNullableField)
                sb.AppendLine("#nullable enable");

            sb.AppendLine($"public {type} {name}");
            sb.AppendLine("{");
            sb.AppendLine(GetterCode(field));
            sb.AppendLine(SetterCode(field, nullableEnabled));
            sb.Append("}");

            if (isNullableField)
            {
                sb.AppendLine();
                sb.Append("#nullable restore");
            }

            return sb.ToString();
        }

        private static string GetterCode(IFieldSymbol field)
        {
            return Text.Indent($"get => {field.Name};");
        }

        private static string SetterCode(IFieldSymbol field, bool nullableEnabled)
        {
            var sb = new StringBuilder();
            sb.AppendLine(Text.Indent("set"));
            sb.AppendLine(Text.Indent("{"));

            var changingAttribute = GetAttribute<INotifyPropertyChangingAttribute>(field);
            var changedAttribute = GetAttribute<INotifyPropertyChangedAttribute>(field);

            var nullSafe = !nullableEnabled || field.Type.HasNullableAnnotation();

            if (changingAttribute is null)
                sb.AppendLine(SetterCodeWithoutChangingEvent(field, changedAttribute, nullSafe));
            else if (!changingAttribute.EqualityCheck)
                sb.AppendLine(SetterCodeWithoutChangingEqualityCheck(field, changingAttribute, changedAttribute, nullSafe));
            else
                sb.AppendLine(SetterCodeWithChangingEqualityCheck(field, changingAttribute, changedAttribute, nullSafe));

            sb.Append(Text.Indent("}"));
            return sb.ToString();
        }

        private static string SetterCodeWithoutChangingEvent(
            IFieldSymbol field, INotifyPropertyChangedAttribute changedAttribute, bool nullSafe)
        {
            var sb = new StringBuilder();
            var propertyName = Snippets.PropertyNameFromField(field);

            if (changedAttribute?.EqualityCheck is true)
            {
                const int tab = 3;
                sb.AppendLine(InequalityConditionCode(field, nullSafe));
                sb.AppendLine(Text.Indent("{", tab - 1));
                sb.AppendLine(SetField(field.Name, tab));
                AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab);
                sb.AppendLine();
                sb.Append(Text.Indent("}", tab - 1));
            }
            else
            {
                const int tab = 2;
                sb.AppendLine(SetField(field.Name, tab));
                AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab);
            }
            return sb.ToString();
        }

        private static string SetterCodeWithoutChangingEqualityCheck(
            IFieldSymbol field, INotifyPropertyChangingAttribute changingAttribute, INotifyPropertyChangedAttribute changedAttribute, bool nullSafe)
        {
            const int tab = 2;

            string tempVar = null;
            var sb = new StringBuilder();
            var propertyName = Snippets.PropertyNameFromField(field);

            AppendRaiseChangingEvent(sb, changingAttribute, propertyName, tab);
            sb.AppendLine();

            if (changedAttribute?.EqualityCheck is true)
            {
                tempVar = Snippets.UnconflictingVariable(field.ContainingType);
                sb.AppendLine(Text.Indent($"var {tempVar} = {field.Name};", tab));
            }

            sb.Append(SetField(field.Name, tab));

            if (changedAttribute != null)
            {
                sb.AppendLine();
                if (!changedAttribute.EqualityCheck)
                    AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab);
                else
                {
                    sb.AppendLine(InequalityConditionCode(field, nullSafe, $"{tempVar}"));
                    sb.AppendLine(Text.Indent("{", tab));
                    AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab + 1);
                    sb.AppendLine();
                    sb.Append(Text.Indent("}", tab));
                }
            }
            return sb.ToString();
        }

        private static string SetterCodeWithChangingEqualityCheck(
            IFieldSymbol field, INotifyPropertyChangingAttribute changingAttribute, INotifyPropertyChangedAttribute changedAttribute, bool nullSafe)
        {
            var sb = new StringBuilder();
            var propertyName = Snippets.PropertyNameFromField(field);

            const int tab = 3;
            sb.AppendLine(InequalityConditionCode(field, nullSafe));
            sb.AppendLine(Text.Indent("{", tab - 1));
            AppendRaiseChangingEvent(sb, changingAttribute, propertyName, tab);

            if (!(changedAttribute?.EqualityCheck is false))
                sb.AppendLine(SetField(field.Name, tab));

            if (changedAttribute?.EqualityCheck is true)
                AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab);

            sb.AppendLine(Text.Indent("}", tab - 1));
            if (changedAttribute?.EqualityCheck is false)
            {
                sb.AppendLine(SetField(field.Name, tab - 1));
                AppendRaiseChangedEvent(sb, changedAttribute, propertyName, tab - 1);
            }
            return sb.ToString().TrimEnd();
        }

        private static string InequalityConditionCode(IFieldSymbol symbol, bool nullSafe, string other = "value")
        {
            var member = FieldSymbolInfo.Create(symbol);
            return Text.Indent(
                $"if ({Snippets.InequalityCheck(member, other, nullSafe, null)})", 2);
        }

        private static T GetAttribute<T>(IFieldSymbol field) where T : class
        {
            var attData = field.AttributesOfType<T>()
                .FirstOrDefault();

            if (AttributeFactory.TryCreate<T>(attData, out var att))
                return att;
            return null;
        }

        private static string SetField(string fieldName, int tabCount)
        {
            return Text.Indent($"{fieldName} = value;", tabCount);
        }

        private static void AppendRaiseChangingEvent(StringBuilder sb, INotifyPropertyChangingAttribute attribute, string propName, int tabCount)
        {
            sb.Append(ChangingEventCall(propName, tabCount));
            if (attribute.RelatedProperties.Any())
            {
                foreach (var prop in attribute.RelatedProperties)
                {
                    sb.AppendLine();
                    sb.Append(ChangingEventCall(prop, tabCount));
                }
            }
        }


        private static void AppendRaiseChangedEvent(StringBuilder sb, INotifyPropertyChangedAttribute attribute, string propName, int tabCount)
        {
            sb.Append(ChangedEventCall(propName, tabCount));
            if (attribute.RelatedProperties.Any())
            {
                foreach (var prop in attribute.RelatedProperties)
                {
                    sb.AppendLine();
                    sb.Append(ChangedEventCall(prop, tabCount));
                }
            }
        }

        private static string ChangingEventCall(string propName, int tabCount)
        {
            return Text.Indent($"{PropertyChangingNotifyMethod}(\"{propName}\");", tabCount);
        }

        private static string ChangedEventCall(string propName, int tabCount)
        {
            return Text.Indent($"{PropertyChangedNotifyMethod}(\"{propName}\");", tabCount);
        }
    }
}

#pragma warning restore IDE0130