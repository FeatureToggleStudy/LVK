using System;

namespace LVK.Reflection
{
    internal class DummyTypeHelper : ITypeHelper
    {
        public string TryGetNameOf(Type type, NameOfTypeOptions options = NameOfTypeOptions.Default) => type.FullName;
    }
}