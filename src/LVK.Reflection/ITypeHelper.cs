using System;

using JetBrains.Annotations;

namespace LVK.Reflection
{
    [PublicAPI]
    public interface ITypeHelper
    {
        [CanBeNull]
        string TryGetNameOf([NotNull] Type type, NameOfTypeOptions options = NameOfTypeOptions.Default);
    }
}