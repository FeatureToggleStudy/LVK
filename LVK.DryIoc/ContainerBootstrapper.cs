using System;
using System.Collections.Generic;

using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [UsedImplicitly]
    internal class ContainerBootstrapper : IContainerBootstrapperRegister
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<Type> _AlreadyBootstrapped = new HashSet<Type>();

        public bool TryAddBootstrapper<T>() => _AlreadyBootstrapped.Add(typeof(T));
    }
}