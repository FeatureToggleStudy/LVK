using System;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    internal interface IServicesBootstrapperRegister
    {
        bool TryAdd([NotNull] Type servicesBootstrapperType);
        bool IsRegistered([NotNull] Type servicesBootstrapperType);
    }
}