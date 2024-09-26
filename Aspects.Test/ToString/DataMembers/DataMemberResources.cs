#pragma warning disable

namespace Aspects.Test.ToString.DataMembers
{
    public static class DataMemberResources
    {
        public static readonly Type[] MustUseField = 
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Field),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Field),
        };

        public static readonly Type[] MustUseProperty =
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_DataMember),
            typeof(ClassWithGeneratedProperty_DataMemberKind_DataMember),
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Property),
            typeof(ClassWithQueryProperty_WithToStringInclude),
        };

        public static readonly Type[] MustBeIgnored =
        {
            typeof(ClassWithQueryProperty_WithDefaultSettings),
            typeof(ClassWithQueryProperty_WithDataMemberKindProperty),
            typeof(ClassWithConstantField),
            typeof(ClassWithConstantField_WithToStringInclude),
            typeof(ClassWithStaticField),
            typeof(ClassWithStaticField_WithToStringInclude),
            typeof(ClassWithEvent),
        };
    }

    [AutoToString]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_DataMember
    {
        public int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoToString(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Field
    {
        public int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoToString(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Property
    {
        public int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoToString(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property
    {
        public int _field;

        public int Property => _field;
    }

    [AutoToString(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property
    {
        public int? _field;

        public int Property => _field ?? 0;
    }

    [AutoToString(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Property
    {
        [NotifyPropertyChanged]
        public int _property;
    }

    [AutoToString(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Field
    {
        [NotifyPropertyChanged]
        public int _field;
    }

    [AutoToString]
    public partial class ClassWithGeneratedProperty_DataMemberKind_DataMember
    {
        [NotifyPropertyChanged]
        public int _property;
    }

    [AutoToString]
    public partial class ClassWithQueryProperty_WithDefaultSettings
    {
        public int Property => 3;
    }

    [AutoToString(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithQueryProperty_WithDataMemberKindProperty
    {
        public int Property => 3;
    }


    public partial class ClassWithQueryProperty_WithToStringInclude
    {
        [ToString]
        public int Property => 3;
    }

    [AutoToString]
    public partial class ClassWithConstantField
    {
        public const int _field = 3;
    }

    public partial class ClassWithConstantField_WithToStringInclude
    {
        [ToString]
        public const int _field = 3;
    }

    [AutoToString]
    public partial class ClassWithStaticField
    {
        public static int _field = 3;
    }

    public partial class ClassWithStaticField_WithToStringInclude
    {
        [ToString]
        public static int _field = 3;
    }

    [AutoToString]
    public partial class ClassWithEvent
    {
        public event EventHandler _field;
    }
}

#pragma warning restore