using System;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Reflection
{
    internal class ReflectionContainerInitializer : IContainerInitializer
    {
        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        public ReflectionContainerInitializer([NotNull] ITypeHelper typeHelper)
        {
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
        }

        public void Initialize()
        {
            TypeHelper.Instance = _TypeHelper;
        }
    }
}