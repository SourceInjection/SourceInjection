using Aspects.Attributes;

#pragma warning disable

namespace Aspects.Test.Equals.BaseCall
{
    using BaseCall = Attributes.BaseCall;

    [AutoEquals]
    public partial class ClassEmpty { }

    public partial class ClassEmpty_NoAttribute { }

    public partial class ClassWithPropery_ThatHasEqualsAttribute
    {
        [Equals]
        public ClassEmpty Property { get; } = null!;
    }

    public partial class ClassWithEqualsOverride
    {
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }
    }

    [AutoEquals]
    public partial class ClassWithBase_ThatHasAutoEquals : ClassEmpty { }

    [AutoEquals(baseCall: BaseCall.Off)]
    public partial class ClassEmptyWithBase_ThatHasAutoEquals_WithBaseCallOff : ClassEmpty { }

    [AutoEquals(baseCall: BaseCall.On)]
    public partial class ClassEmpty_WithBaseCallOn { }

    [AutoEquals(baseCall: BaseCall.On)]
    public readonly partial struct NonReferenceTypeEmpty_WithBaseCallOn { }

    [AutoEquals]
    public partial class ClassWithBase_ThatHasEqualsOnMember : ClassWithPropery_ThatHasEqualsAttribute { }

    [AutoEquals]
    public partial class ClassWithBase_ThatHasNoEqualsOverride : ClassEmpty_NoAttribute { }

    [AutoEquals]
    public partial class ClassWithBase_ThatHasEqualsOverride : ClassWithEqualsOverride { }
}

#pragma warning restore
