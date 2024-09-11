namespace Aspects
{
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
