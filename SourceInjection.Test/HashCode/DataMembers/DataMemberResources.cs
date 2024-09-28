#pragma warning disable

namespace SourceInjection.Test.HashCode.DataMembers
{
    public static class DataMemberResources
    {
        public static readonly Type[] MustUseField = 
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_DataMember),
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Field),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Field),
            typeof(ClassWithGeneratedProperty_DataMemberKind_DataMember),
        };

        public static readonly Type[] MustUseProperty =
        {
            typeof(ClassWithPropertyLinkedField_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property),
            typeof(ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property),
            typeof(ClassWithGeneratedProperty_DataMemberKind_Property),
            typeof(ClassWithQueryProperty_WithHashCodeInclude),
        };

        public static readonly Type[] MustBeIgnored =
        {
            typeof(ClassWithQueryProperty_WithDefaultSettings),
            typeof(ClassWithQueryProperty_WithDataMemberKindProperty),
            typeof(ClassWithConstantField),
            typeof(ClassWithConstantField_WithHashCodeInclude),
            typeof(ClassWithStaticField),
            typeof(ClassWithStaticField_WithHashCodeInclude),
            typeof(ClassWithEvent),
        };
    }

    [AutoHashCode]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_DataMember
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Field
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_DataMemberKind_Property
    {
        private int _field;

        public int Property
        {
            get => _field;
            set => _field = value;
        }
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_DataMemberKind_Property
    {
        private int _field;

        public int Property => _field;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithPropertyLinkedField_ArrowFunction_Coalesce_DataMemberKind_Property
    {
        private int? _field;

        public int Property => _field ?? 0;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Property)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Property
    {
        [NotifyPropertyChanged]
        private int _property;
    }

    [AutoHashCode(dataMemberKind: DataMemberKind.Field)]
    public partial class ClassWithGeneratedProperty_DataMemberKind_Field
    {
        [NotifyPropertyChanged]
        private int _field;
    }

    [AutoHashCode]
    public partial class ClassWithGeneratedProperty_DataMemberKind_DataMember
    {
        [NotifyPropertyChanged]
        private int _field;
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


    public partial class ClassWithQueryProperty_WithHashCodeInclude
    {
        [HashCode]
        public int Property => 3;
    }

    [AutoHashCode]
    public partial class ClassWithConstantField
    {
        public const int _field = 3;
    }

    public partial class ClassWithConstantField_WithHashCodeInclude
    {
        [HashCode]
        public const int _field = 3;
    }

    [AutoHashCode]
    public partial class ClassWithStaticField
    {
        static int _field = 3;
    }

    public partial class ClassWithStaticField_WithHashCodeInclude
    {
        [HashCode]
        static int _field = 3;
    }

    [AutoHashCode]
    public partial class ClassWithEvent
    {
        public event EventHandler _field;
    }
}

#pragma warning restore