﻿using Aspects.Attributes;

#pragma warning disable

namespace Aspects.Test.HashCode.BaseCall
{
    using BaseCall = Attributes.BaseCall;

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