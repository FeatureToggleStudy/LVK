using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [UsedImplicitly]
    internal class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            global::System.Console.WriteLine("HERE");
            container.Register<IConsoleApplicationEntryPoint, ConsoleApplicationEntryPoint>();
        }
    }
}