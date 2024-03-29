﻿using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.DataStructures
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Register<ITopologicalSorter, TopologicalSorter>();
        }
    }
}