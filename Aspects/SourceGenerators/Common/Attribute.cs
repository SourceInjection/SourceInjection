
using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class Attribute
    {
        public static T Create<T>(AttributeData data)
        {
            string name = data.AttributeClass.ToDisplayString();
            var type = Type.GetType(name) 
                ?? throw new InvalidOperationException($"could not find type with name '{name}'");

            return (T)Activator.CreateInstance(type, data.ConstructorArguments
                .Select(arg => SelectValue(arg)).ToArray());
        }

        private static object SelectValue(TypedConstant constant)
        {
            if (constant.Kind == TypedConstantKind.Enum)
                return EnumFromConstant(constant);
            if (constant.Kind == TypedConstantKind.Array)
                return constant.Values;
            return constant.Value;
        }

        private static object EnumFromConstant(TypedConstant constant)
        {
            if (constant.Value is null)
                return null;

            var type = Type.GetType(constant.Type.ToDisplayString());
            return Enum.Parse(type, constant.Value.ToString());
        }
    }
}
