#pragma warning disable

namespace Aspects.Test.HashCode.DataMembers
{
    using NullSafety = NullSafety;

    [AutoHashCode]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_DataMember
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Field
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Property
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property
    {
        private int _int;

        public int Int => _int;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property
    {
        private int? _int;

        public int Int => _int ?? 0;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Property
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Field
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoHashCode]
    public partial class ClassWithGeneratedProperty_DataMemberKind_DataMember
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoHashCode(DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_WithFieldConfiguration
    {
        [HashCode]
        [NotifyPropertyChanged]
        private object _object = null!;
    }

    [AutoHashCode]
    public partial class ClassWithQueryProperty_WithDefaultSettings
    {
        public int Property => 3;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithQueryProperty_WithDataMemberKindProperty
    {
        public int Property => 3;
    }


    public partial class ClassWithQueryProperty_WithEqualsInclude
    {
        [HashCode]
        public int Property => 3;
    }

    [AutoHashCode]
    public partial class ClassWithConstantField
    {
        public const int _int = 3;
    }

    public partial class ClassWithConstantField_WithEqualsInclude
    {
        [HashCode]
        public const int _int = 3;
    }

    [AutoHashCode]
    public partial class ClassWithStaticField
    {
        static int s_int = 3;
    }

    public partial class ClassWithStaticField_WithEqualsInclude
    {
        [HashCode]
        static int s_int = 3;
    }

    [AutoHashCode]
    public partial class ClassWithEvent
    {
        public event EventHandler Event;
    }
}

#pragma warning restore