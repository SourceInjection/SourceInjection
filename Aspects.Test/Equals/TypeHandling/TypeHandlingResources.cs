﻿using Aspects.Attributes;

#pragma warning disable

namespace Aspects.Test.Equals.TypeHandling
{
    [AutoEquals]
    public partial class ClassEmpty { }

    [AutoEquals]
    public partial struct StructEmpty { }

#nullable disable
    [AutoEquals]
    public partial struct StructEmpty_NullableOff { }

#nullable restore

}

#pragma warning restore