using Aspects.Attributes.Interfaces;
using System.Reflection;

namespace Aspects.Test.CompileTime.PropertyEvents
{
    internal static class PropertyEventTestSources
    {

        private static IEnumerable<FieldInfo> GetFieldsWith<T>()
        {
            var name = typeof(T).FullName;
            return typeof(PropertyEventTestClass).GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => f.CustomAttributes.Any(a => a.AttributeType.FindInterfaces((t, o) => t.FullName == name, null).Length > 0));
        }

        public static FieldInfo[] FieldsWithINotifyPropertyChangedAttribute
            = GetFieldsWith<INotifyPropertyChangedAttribute>().ToArray();

        public static FieldInfo[] FieldsWithINotifyPropertyChangingAttribute
            = GetFieldsWith<INotifyPropertyChangingAttribute>().ToArray();

        public static FieldInfo[] AllAttributedFields
            = GetFieldsWith<IPropertyEventGenerationAttribute>().ToArray();

        public static FieldInfo[] FieldsWithBothAttributes
            = GetFieldsWith<INotifyPropertyEventsAttribute>().ToArray();
    }
}
