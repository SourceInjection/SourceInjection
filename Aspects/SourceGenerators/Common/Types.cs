using Aspects.Util;
using System;
using System.Linq;

namespace Aspects.SourceGenerators.Common
{
    internal static class Types
    {
        public static Predicate<TypeInfo> With<T>() where T : Attribute
        {
            return WithAttribute(typeof(T).FullName);
        }

        public static Predicate<TypeInfo> WithAttribute(string attributeFullname)
        {
            return (TypeInfo type) => type.Symbol.HasAttribute(attributeFullname);
        }

        public static Predicate<TypeInfo> WithMembersWith<T>() where T : Attribute
        {
            return WithMembersWithAttribute(typeof(T).FullName);
        }

        public static Predicate<TypeInfo> WithMembersWithAttribute(string attributeFullname)
        {
            return (TypeInfo type) => type.Symbol.GetMembers().Any(m => m.HasAttribute(attributeFullname));
        }
    }
}
