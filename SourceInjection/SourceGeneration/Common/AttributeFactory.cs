﻿using Microsoft.CodeAnalysis;
using System;
using System.Linq;
using System.Reflection;

namespace SourceInjection.SourceGeneration.Common
{
    internal static class AttributeFactory
    {
        public static bool TryCreate<T>(AttributeData data, out T result)
        {
            try
            {
                result = Create<T>(data);
                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static T Create<T>(AttributeData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var name = data.AttributeClass.ToDisplayString();
            var type = Type.GetType(name);


            var args = data.ConstructorArguments
                    .Select(arg => SelectValue(arg))
                    .ToArray();

            try
            {
                return (T)Activator.CreateInstance(type, args);
            }
            catch
            {
#pragma warning disable S3011
                return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null);
#pragma warning restore S3011
            }
        }

        private static object SelectValue(TypedConstant constant)
        {
            if (constant.IsNull)
                return null;

            if (constant.Kind == TypedConstantKind.Enum)
                return EnumFromConstant(constant);
            if (constant.Kind == TypedConstantKind.Type)
                return (constant.Value as INamedTypeSymbol)?.ToDisplayString();
            if (constant.Kind == TypedConstantKind.Array)
                return constant.Values.Select(v => SelectValue(v)).ToArray();
            return constant.Value;
        }

        private static object EnumFromConstant(TypedConstant constant)
        {
            if (constant.Value is null)
                return null;

            var type = GetType(constant.Type.ToDisplayString());

            return Enum.Parse(type, constant.Value.ToString());
        }

        private static Type GetType(string name)
        {
            var type = Type.GetType(name);
            if (type != null)
                return type;

            if (name == typeof(Accessibility).FullName)
                return typeof(Accessibility);

            // TODO: find a way to load all types of Microsoft.CodeAnalysis

            throw new TypeLoadException($"Could not find type '{name}'");
        }
    }
}
