using Aspects.Attributes;

namespace Aspects.Test.HashCode.Code
{
    [AutoHashCode]
    public partial class ClassEmpty { }

    [AutoHashCode]
    public partial class SubClassFromClassEmpty : ClassEmpty { }

    [AutoHashCode]
    public partial struct StructEmpty { }

    [AutoHashCode(baseCall: BaseCall.On)]
    public partial struct StructEmpty_WithBaseCallOn { }
}
