﻿using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Commands
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Logging.ServicesBootstrapper>();
            container.Bootstrap<Reflection.ServicesBootstrapper>();

            container.Register<ICommandDispatcher, CommandDispatcher>();
        }
    }
}