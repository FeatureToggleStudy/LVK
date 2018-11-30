using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    internal class ServicesBootstrapperRegister : IServicesBootstrapperRegister
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<Type> _KnownTypes = new HashSet<Type>();

        public bool TryAdd(Type servicesBootstrapperType) => _KnownTypes.Add(servicesBootstrapperType);
        public bool IsRegistered(Type servicesBootstrapperType) => _KnownTypes.Contains(servicesBootstrapperType);
    }
}