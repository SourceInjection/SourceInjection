using System;

namespace SourceInjection
{
    /// <summary>
    /// Defines the accessability of types and members.<br/>
    /// See <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers">Access Modifiers</see>.
    /// </summary>
    [Flags]
    public enum Accessibilities
    {
        /// <summary>
        /// No modifier is present.
        /// </summary>
        None = 0,
        /// <summary>
        /// <see langword="public"/> modifier is present.
        /// </summary>
        Public = 1,
        /// <summary>
        /// <see langword="internal"/> modifier is present.
        /// </summary>
        Internal = 2,
        /// <summary>
        /// <see langword="protected"/> modifier is present.
        /// </summary>
        Protected = 4,
        /// <summary>
        /// <see langword="private"/> modifier is present.
        /// </summary>
        Private = 8,
        /// <summary>
        /// <see langword="protected"/> and <see langword="internal"/> modifiers are present.
        /// </summary>
        ProtectedInternal = 16,
        /// <summary>
        /// <see langword="protected"/> and <see langword="private"/> modifiers are present.
        /// </summary>
        ProtectedPrivate = 32,

        All = Public | ProtectedInternal | Internal | Protected | ProtectedPrivate | Private,

        GreaterPrivate = Public | ProtectedInternal | Internal | Protected | ProtectedPrivate,

        GreaterProtectedPrivate = Public | ProtectedInternal | Internal | Protected,

        GreaterProtected = Public | ProtectedInternal | Internal,

        GreaterInternal = Public | ProtectedInternal,
    }


    /// <summary>
    /// Defines which kind of data member are taken into account during code generation.
    /// </summary>
    public enum DataMemberKind
    {
        /// <summary>
        /// Only fields will be used.
        /// </summary>
        Field = 1,
        /// <summary>
        /// Only properties will be used.
        /// </summary>
        Property = 2,
        /// <summary>
        /// Properties and fields will be used and automatically merged.
        /// </summary>
        DataMember = Field | Property
    }

    /// <summary>
    /// Defines how operations are generated regarding to null safety.
    /// </summary>
    public enum NullSafety
    {
        /// <summary>
        /// If nullable feature is used null safe operations will be applied depending on the type.
        /// If not used all reference type operations are null safe by default.
        /// Does also take System.Diagnostics.CodeAnalysis attributes MaybeNull and NotNull into account.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// If possible null safe operations are used.
        /// </summary>
        On = 1,
        /// <summary>
        /// operations are not nullsafe.
        /// </summary>
        Off = 2,
    }

    /// <summary>
    /// Defines how base calls are included during code generation.
    /// </summary>
    public enum BaseCall
    {
        /// <summary>
        /// <see langword="base"/> method is called if <see langword="base"/> <see langword="class"/> does overwrite it.
        /// </summary>
        Auto = 0,
        /// <summary>
        /// <see langword="base"/> method is called if possible.
        /// </summary>
        On = 1,
        /// <summary>
        /// <see langword="base"/> method is not called.
        /// </summary>
        Off = 2,
    }
}
