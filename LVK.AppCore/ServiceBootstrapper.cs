using System;
using System.Diagnostics;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.AppCore
{
    [PublicAPI]
    public class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));
        }
    }
}