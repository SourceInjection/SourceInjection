
#pragma warning disable

using Aspects;

namespace Aspects.Test.Equals.BaseCall
{
    using BaseCall = Aspects.BaseCall;

    public static class BaseCallResources
    {
        public static readonly Type[] MustCallBase =
        {
            typeof(ClassWithBase_ThatHasAutoEquals),
            typeof(ClassEmpty_WithBaseCallOn),
            typeof(ClassWithBase_ThatHasEqualsOnMember),
            typeof(ClassWithBase_ThatHasEqualsOverride),
        };

        public static readonly Type[] MustNotCallBase =
        {
            typeof(ClassEmptyWithBase_ThatHasAutoEquals_WithBaseCallOff),
            typeof(NonReferenceTypeEmpty_WithBaseCallOn),
            typeof(ClassWithBase_ThatHasNoEqualsOverride),
        };
    }

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
