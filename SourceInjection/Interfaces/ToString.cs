using System;

namespace SourceInjection.Interfaces
{
    public interface IAutoToStringAttribute 
    {
        /// <summary>
        /// Defines which data members are used to generate <see cref="object.ToString"/>.
        /// </summary>
        DataMemberKind DataMemberKind { get; }

        /// <summary>
        /// Defines which accessibility is required to be used foreach member to be included to generate <see cref="object.ToString"/>.
        /// </summary>
        Accessibility Accessibility { get; }

        /// <summary>
        /// Defines if <see cref="IFormattable.ToString(string, IFormatProvider)"/> is implemented.
        /// </summary>
        bool Formatable { get; }
    }

    public interface IToStringAttribute 
    {
        /// <summary>
        /// Defines the name which is used to represent the member.
        /// </summary>
        string Label { get; }

        /// <summary>
        /// Defines the format which will be handover to the string representation of the targeted object.
        /// </summary>
        string Format { get; }

        /// <summary>
        /// Defines the <see cref="IFormatProvider"/> which is passed to the <see cref="IFormattable.ToString(string, IFormatProvider)"/> method.
        /// </summary>
        string FormatProvider { get; }

        /// <summary>
        /// Defines if the <see cref="IFormattable"/> from <see cref="IFormattable.ToString(string, IFormatProvider)"/> is passed to the members ToString method.
        /// </summary>
        bool UseArgumentFormatProvider { get; }
    }

    public interface IToStringExcludeAttribute { }
}
