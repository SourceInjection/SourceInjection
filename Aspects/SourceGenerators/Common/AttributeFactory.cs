using Microsoft.CodeAnalysis;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class AttributeFactory
    {
        /// <summary>
        /// Uses <see cref="Create{T}(AttributeData)"/> to get an instance of an <see cref="Attribute"/> 
        /// and handles <see cref="Exception"/>s thrown by <see cref="Create{T}(AttributeData)"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to wich the instance is casted.</typeparam>
        /// <param name="data">The <see cref="AttributeData"/> from which the <see cref="Type"/> is extracted.</param>
        /// <param name="result">The resulting <see cref="Attribute"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Attribute"/> creation is successful and 
        /// <see langword="false"/> if an <see cref="Exception"/> is thrown.</returns>
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

        /// <summary>
        /// Creates an instance of an <see cref="Attribute"/> and casts it to T.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> to wich the instance is casted.</typeparam>
        /// <param name="data">The <see cref="AttributeData"/> from which the <see cref="Type"/> is extracted.</param>
        /// <returns>The instance of an <see cref="Attribute"/> casted to T.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="System.Reflection.TargetInvocationException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="TypeLoadException"></exception>
        /// <exception cref="System.IO.FileLoadException"></exception>
        /// <exception cref="BadImageFormatException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="MethodAccessException"></exception>
        /// <exception cref="MemberAccessException"></exception>
        /// <exception cref="System.Runtime.InteropServices.InvalidComObjectException"></exception>
        /// <exception cref="MissingMethodException"></exception>
        /// <exception cref="System.Runtime.InteropServices.COMException"></exception>
        public static T Create<T>(AttributeData data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            string name = data.AttributeClass.ToDisplayString();
            var type = Type.GetType(name);

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
