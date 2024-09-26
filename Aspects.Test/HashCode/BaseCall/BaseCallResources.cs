#pragma warning disable

using Aspects;

namespace Aspects.Test.HashCode.BaseCall
{
    using BaseCall = Aspects.BaseCall;

    public static class BaseCallResources
    {
        public static readonly Type[] MustCallBase =
        {
            typeof(ClassEmpty_WithBaseCallOn),
            typeof(SubClassOfClassEmpty),
            typeof(SubClassOfClassEmpty_WithBaseCallOn)
        };

        public static readonly Type[] MustNotCallBase =
        {
            typeof(ClassEmpty),
            typeof(StructEmpty),
            typeof(StructEmpty_WithBaseCallOn),
            typeof(SubClassOfClassEmpty_WithBaseCallOff)
        };
    }

    [AutoHashCode]
    public partial class ClassEmpty { }

    [AutoHashCode(baseCall: BaseCall.On)]
    public partial class ClassEmpty_WithBaseCallOn { }

    [AutoHashCode]
    public partial class SubClassOfClassEmpty : ClassEmpty { }

    [AutoHashCode]
    public partial struct StructEmpty { }

    [AutoHashCode(baseCall: BaseCall.On)]
    public partial struct StructEmpty_WithBaseCallOn { }

    [AutoHashCode(baseCall: BaseCall.On)]
    public partial class SubClassOfClassEmpty_WithBaseCallOn : ClassEmpty { }

    [AutoHashCode(baseCall: BaseCall.Off)]
    public partial class SubClassOfClassEmpty_WithBaseCallOff : ClassEmpty { }
}

#pragma warning restore