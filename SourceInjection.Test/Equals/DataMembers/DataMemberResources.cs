#pragma warning disable

namespace SourceInjection.Test.Equals.DataMembers
{
    public static class DataMemberResources
    {
        public static readonly Type[] MustUseField =
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_DataMember),
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Field),
            typeof(ClassWithGeneratedProperty_DataMemberKind_DataMember),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Field),
        };

        public static readonly Type[] MustUseProperty =
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Property),
            typeof(ClassWithQueryProperty_WithEqualsInclude),
        };

        public static readonly Type[] MustBeIgnored =
        {
            typeof(ClassWithEvent),
            typeof(ClassWithConstantField),
            typeof(ClassWithConstantField_WithEqualsInclude),
            typeof(ClassWithStaticField),
            typeof(ClassWithStaticField_WithEqualsInclude),
            typeof(ClassWithQueryProperty_WithDataMemberKindProperty),
            typeof(ClassWithQueryProperty_WithDefaultSettings)
        };
    }

    [AutoEquals]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_DataMember
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Field
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Property
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property
    {
        private int _field;

        public int Property => _field;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property
    {
        private int? _field;

        public int Property => _field ?? 0;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Property
    {
        [NotifyPropertyChanged(equalityCheck: false)]
        private int _property;
    }

    [AutoEquals(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Field
    {
        [NotifyPropertyChanged(equalityCheck: false)]
        private int _field;
    }

    [AutoEquals]
    public partial class ClassWithGeneratedProperty_DataMemberKind_DataMember
    {
        [NotifyPropertyChanged(equalityCheck: false)]
        private int _field;
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
        public const int _field = 3;
    }

    public partial class ClassWithConstantField_WithEqualsInclude
    {
        [Equals]
        public const int _field = 3;
    }

    [AutoEquals]
    public partial class ClassWithStaticField
    {
        static int _field = 3;
    }

    public partial class ClassWithStaticField_WithEqualsInclude
    {
        [Equals]
        static int _field = 3;
    }

    [AutoEquals]
    public partial class ClassWithEvent
    {
        public event EventHandler _field;
    }
}

#pragma warning restore