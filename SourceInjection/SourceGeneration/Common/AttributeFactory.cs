using Microsoft.CodeAnalysis;
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
            var type = TypeLoader.GetType(name);


            var args = data.ConstructorArguments
                    .Select(arg => SelectValue(arg))
                    .ToArray();

            if(args.Length > 0 && data.ConstructorArguments[args.Length - 1].Kind == TypedConstantKind.Array && args[args.Length - 1] == null)
                Array.Resize(ref args, args.Length - 1);

            try
            {
                return (T)Activator.CreateInstance(type, args);
            }
            catch (Exception ex)
            {
                try
                {
#pragma warning disable S3011
                    return (T)Activator.CreateInstance(type, BindingFlags.Instance | BindingFlags.NonPublic, null, args, null);
#pragma warning restore S3011
                }
                catch
                {
                    throw ex;
                }
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
            {
                var objects = constant.Values.Select(v => SelectValue(v)).ToArray();
                if(objects.Length == 0 || Array.TrueForAll(objects, o => o == null))
                    return null;

                var elem = Array.Find(objects, o => o != null);

                if (elem is string) return Cast<string>(objects);
                if (elem is decimal) return Cast<decimal>(objects);
                if (elem is double) return Cast<double>(objects);
                if (elem is float) return Cast<float>(objects);
                if (elem is long) return Cast<long>(objects);
                if (elem is int) return Cast<int>(objects);
                if (elem is short) return Cast<short>(objects);
                if (elem is sbyte) return Cast<sbyte>(objects);
                if (elem is ulong) return Cast<ulong>(objects);
                if (elem is uint) return Cast<uint>(objects);
                if (elem is ushort) return Cast<ushort>(objects);
                if (elem is byte) return Cast<byte>(objects);
                if (elem is char) return Cast<char>(objects);
                if (elem is bool) return Cast<bool>(objects);

                return null;
            }
            return constant.Value;
        }

        private static T[] Cast<T>(object[] values)
            => values.Select(v => (T)v).ToArray();

        private static object EnumFromConstant(TypedConstant constant)
        {
            if (constant.Value is null)
                return null;

            var type = TypeLoader.GetType(constant.Type.ToDisplayString());

            return Enum.Parse(type, constant.Value.ToString());
        }
    }
}
