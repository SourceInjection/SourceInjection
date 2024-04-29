using Aspects.Attributes;
using Aspects.SourceGenerators.Base;
using Aspects.SourceGenerators.Common;
using Aspects.SourceGenerators.Queries;
using Aspects.SyntaxReceivers;
using Aspects.Util;
using Microsoft.CodeAnalysis;
using System.Linq;
using System.Text;
using TypeInfo = Aspects.SourceGenerators.Common.TypeInfo;

namespace Aspects.SourceGenerators
{
    [Generator]
    internal class PropertyChangedSourceGenerator : TypeSourceGeneratorBase
    {
        protected override string Name { get; } = "PropertyChanged";

        protected override TypeSyntaxReceiver SyntaxReceiver { get; }
            = new TypeSyntaxReceiver(Types.WithMembersWithAttributeOfType<NotifyPropertyChangedAttribute>());


        protected override string Dependencies(TypeInfo typeInfo)
        {
            return "using System.ComponentModel;";
        }

        protected override string ClassBody(TypeInfo typeInfo)
        {
            var sb = new StringBuilder();

            if (!typeInfo.Members(true).Any(sy => sy.Name == "PropertyChanged"))
            {
                sb.AppendLine("public event PropertyChangedEventHandler PropertyChanged;");
                sb.AppendLine();
            }

            var attributedFields = typeInfo.Symbol.GetMembers()
                .OfType<IFieldSymbol>()
                .Where(a => a.HasAttributeOfType<NotifyPropertyChangedAttribute>())
                .ToArray();

            sb.Append(PropertyCode(attributedFields[0]));
            for (int i = 1; i < attributedFields.Length; i++)
            {
                sb.AppendLine();
                sb.AppendLine();
                sb.Append(PropertyCode(attributedFields[i]));
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
            var name = CodeSnippets.PropertyNameFromField(field);
            var sb = new StringBuilder();

            var attName = typeof(NotifyPropertyChangedAttribute).FullName;
            var attData = field.GetAttributes()
                .First(a => a.AttributeClass.ToDisplayString() == attName);

            var attribute = Attribute.Create<NotifyPropertyChangedAttribute>(attData);
            if (attribute.Visibility == Attributes.Accessibility.Public)
                sb.AppendLine("\tset");
            else
                sb.AppendLine($"\t{attribute.Visibility.ToDisplayString()} set");

            sb.AppendLine("\t{");
            if (!attribute.EqualityCheck)
                sb.AppendLine(SetField(field.Name, name, 2));
            else
            {
                if (!field.Type.IsReferenceType)
                    sb.AppendLine($"\t\tif (!{field.Name}.{nameof(Equals)}(value))");
                else
                {
                    if (field.Type.IsEnumerable() && !field.Type.OverridesEquals())
                        sb.AppendLine($"\t\tif (!{CodeSnippets.SequenceEqualsMethod(field.Name, "value")}");

                    else sb.AppendLine($"\t\tif (!({field.Name} is null) && !{field.Name}.{nameof(Equals)}(value) " +
                        $"|| {field.Name} is null && !(value is null))");
                }
                sb.AppendLine("\t\t{");
                sb.AppendLine(SetField(field.Name, name, 3));
                sb.AppendLine("\t\t}");
            }
            sb.Append("\t}");

            return sb.ToString();
        }

        private static string SetField(string fieldName, string propName, int indent)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < indent; i++)
                sb.Append('\t');
            sb.AppendLine($"{fieldName} = value;");
            for (int i = 0; i < indent; i++)
                sb.Append('\t');
            sb.Append($"PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof({propName})));");
            return sb.ToString();
        }
    }
}
