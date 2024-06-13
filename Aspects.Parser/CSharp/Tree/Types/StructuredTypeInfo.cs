using Aspects.Parsers.CSharp.Tree.Members;
using System;
using System.Collections.Generic;

namespace Aspects.Parsers.CSharp.Tree.Types
{
    public abstract class StructuredTypeInfo(string name, AccessModifier accessModifier, IReadOnlyList<MemberInfo> members) 
        : TypeInfo(name, accessModifier, members)
    {

        public IReadOnlyList<FieldInfo> Fields { get; } = members.OfType<FieldInfo>().ToArray();

        public IReadOnlyList<PropertyInfo> Properties { get; } = members.OfType<PropertyInfo>().ToArray();

        public IReadOnlyList<MethodInfo> Methods { get; } = members.OfType<MethodInfo>().ToArray();

        public IReadOnlyList<TypeInfo> Types { get; } = members.OfType<TypeInfo>().ToArray();
    }
}
