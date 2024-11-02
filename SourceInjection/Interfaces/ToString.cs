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
        Accessibilities Accessibility { get; }
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
        /// Defines the type name of the <see cref="IFormatProvider"/> which is linked to the members <see cref="IFormattable.ToString(string, IFormatProvider)"/> method.
        /// </summary>
        string FormatProvider { get; }
    }

    public interface IToStringExcludeAttribute { }
}
