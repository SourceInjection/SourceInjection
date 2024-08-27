namespace Aspects.Attributes
{
    public enum DataMemberKind 
    {
        Field = 1,
        Property = 2,      
        DataMember = Field | Property
    }

    public enum NullSafety
    {
        Auto = 0,
        On = 1,
        Off = 2,
    }

    public enum BaseCall
    {
        Auto = 0,
        On = 1,
        Off = 2,
    }
}
