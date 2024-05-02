using Aspects.Attributes.Interfaces;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.Queries;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class PropertyEventSourceGenerator : TypeSourceGeneratorBase
    {
        protected override string Name { get; } = "PropertyEvent";

        protected override TypeSyntaxReceiver SyntaxReceiver { get; } = new TypeSyntaxReceiver(
                    Types.WithMembersWithAttributeOfType<INotifyPropertyChangedAttribute>()
                .Or(Types.WithMembersWithAttributeOfType<INotifyPropertyChangingAttribute>()));


        protected override string Dependencies(TypeInfo typeInfo)
        {
            return "using System.ComponentModel;";
        }

        protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();

            var fields = typeInfo.Symbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(f => f.HasAttributeOfType<INotifyPropertyChangedAttribute>() 
                    || f.HasAttributeOfType<INotifyPropertyChangingAttribute>())
                .ToArray();

            if(Array.Exists(fields, f => f.HasAttributeOfType<INotifyPropertyChangedAttribute>())
                && !typeInfo.Members(true).Any(sy => sy.Name == "PropertyChanged"))
            {
                sb.AppendLine("public event PropertyChangedEventHandler PropertyChanged;");
            }

            if (Array.Exists(fields, f => f.HasAttributeOfType<INotifyPropertyChangingAttribute>())
                && !typeInfo.Members(true).Any(sy => sy.Name == "PropertyChanging"))
            {
                sb.AppendLine("public event PropertyChangingEventHandler PropertyChanging;");
            }

            sb.Append(PropertyCode(fields[0]));
            for (int i = 1; i < fields.Length; i++)
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(PropertyCode(fields[i]));
            }

            return sb.ToString();
        }

        private static string PropertyCode(IFieldSymbol field)
        {
            var name = CodeSnippets.PropertyNameFromField(field);
            var sb = new StringBuilder();

            var type = field.Type.ToDisplayString();
            var enableNull = type.Length > 0 && type[type.Length - 1] == '?';

            if (enableNull)
                sb.AppendLine("#nullable enable");

            sb.AppendLine($"public {type} {name}");
            sb.AppendLine("{");
            sb.AppendLine(GetterCode(field));
            sb.AppendLine(SetterCode(field, name));
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

        private static string SetterCode(IFieldSymbol field, string propertyName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\tset");

            var changingAttribute = GetAttribute<INotifyPropertyChangingAttribute>(field);
            var changedAttribute = GetAttribute<INotifyPropertyChangedAttribute>(field);

            if(changingAttribute is null)
            {
                if(changedAttribute?.EqualityCheck is true)
                {
                    const int tab = 2;
                    sb.AppendLine(EqualityCheck(field));
                    sb.AppendLine("\t{");
                    sb.AppendLine(SetField(field.Name, tab));
                    sb.AppendLine(RaiseChangedEvent(propertyName, tab));
                    sb.AppendLine("\t}");
                }
                else
                {
                    const int tab = 1;
                    sb.AppendLine(SetField(field.Name, tab));
                    sb.AppendLine(RaiseChangedEvent(propertyName, tab));
                }
            }
            else if (!changingAttribute.EqualityCheck)
            {
                sb.AppendLine(RaiseChangingEvent(propertyName, 1));
                sb.AppendLine(SetField(field.Name, 1));

                if(changedAttribute != null)
                {
                    if (!changedAttribute.EqualityCheck)
                        sb.AppendLine(RaiseChangedEvent(propertyName, 1));
                    else
                    {
                        sb.AppendLine(EqualityCheck(field));
                        sb.AppendLine("\t{");
                        sb.AppendLine(RaiseChangedEvent(propertyName, 2));
                        sb.AppendLine("\t}");
                    }
                }
            }
            else
            {
                sb.AppendLine(EqualityCheck(field));
                sb.AppendLine("\t{");
                sb.AppendLine(RaiseChangingEvent(propertyName, 2));
                if (!(changedAttribute?.EqualityCheck is false))
                    sb.AppendLine(SetField(field.Name, 2));
                if(changedAttribute?.EqualityCheck is true)
                    sb.AppendLine(RaiseChangedEvent(propertyName, 2));
                sb.AppendLine("\t}");
                if (changedAttribute?.EqualityCheck is false)
                {
                    sb.AppendLine(SetField(field.Name, 1));
                    sb.AppendLine(RaiseChangedEvent(propertyName, 1));
                }
            }

            return sb.ToString();
        }

        private static string EqualityCheck(IFieldSymbol field)
        {
            if (!field.Type.IsReferenceType)
                return $"\t\tif (!{field.Name}.{nameof(Equals)}(value))";
            else
            {
                if (field.Type.IsEnumerable() && !field.Type.OverridesEquals())
                    return $"\t\tif (!{CodeSnippets.SequenceEqualsMethod(field.Name, "value")}";

                return $"\t\tif (!({field.Name} is null) && !{field.Name}.{nameof(Equals)}(value) " +
                    $"|| {field.Name} is null && !(value is null))";
            }
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
            return Indent(fieldName, tabCount);
        }

        private static string RaiseChangingEvent(string propName, int tabCount)
        {
            return Indent($"PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof({propName})));", tabCount);
        }

        private static string RaiseChangedEvent(string propName, int tabCount)
        {
            return Indent($"PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propName})));", tabCount);
        }

        private static string Indent(string value, int tabCount)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < tabCount; i++)
                sb.Append('\t');
            sb.Append(value);
            return sb.ToString();
        }
    }
}
