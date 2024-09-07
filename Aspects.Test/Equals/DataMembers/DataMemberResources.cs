using Aspects.Attributes;

#pragma warning disable

namespace Aspects.Test.Equals.DataMembers
{
    using NullSafety = Attributes.NullSafety;

    [AutoEquals]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_DataMember
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Field
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Property
    {
        private int _int;

        public int Int
        {
            get => _int;
            set => _int = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property
    {
        private int _int;

        public int Int => _int;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property
    {
        private int? _int;

        public int Int => _int ?? 0;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Property
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Field
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoEquals]
    public partial class ClassWithGeneratedProperty_DataMemberKind_DataMember
    {
        [NotifyPropertyChanged]
        private int _int;
    }

    [AutoEquals(DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_WithFieldConfiguration
    {
        [Equals(nullSafety: NullSafety.On)]
        [NotifyPropertyChanged]
        private object _object = null!;
    }

    [AutoEquals]
    public partial class ClassWithQueryProperty_WithDefaultSettings
    {
        public int Property => 3;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithQueryProperty_WithDataMemberKindProperty
    {
        public int Property => 3;
    }


    public partial class ClassWithQueryProperty_WithEqualsInclude
    {
        [Equals]
        public int Property => 3;
    }

    [AutoEquals]
    public partial class ClassWithConstantField
    {
        public const int _int = 3;
    }

    public partial class ClassWithConstantField_WithEqualsInclude
    {
        [Equals]
        public const int _int = 3;
    }

    [AutoEquals]
    public partial class ClassWithStaticField
    {
        static int s_int = 3;
    }

    public partial class ClassWithStaticField_WithEqualsInclude
    {
        [Equals]
        static int s_int = 3;
    }

    [AutoEquals]
    public partial class ClassWithEvent
    {
        public event EventHandler Event;
    }
}

#pragma warning restore