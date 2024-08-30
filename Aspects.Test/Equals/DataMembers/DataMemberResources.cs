using Aspects.Attributes;

#pragma warning disable CS0659

namespace Aspects.Test.Equals.DataMembers
{
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
}

#pragma warning restore CS0659