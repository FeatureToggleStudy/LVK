using System;
using System.Diagnostics;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Wpf
{
    [PublicAPI]
    public static class WpfAppBootstrapper
    {
        public static void RunWpfMainWindow<T>()
            where T: class, IServicesBootstrapper
        {
            var container = ContainerFactory.Bootstrap<ServicesBootstrapper<T>>();

            try
            {
                var lifetimeManager = container.Resolve<IWpfApplicationLifetimeManager>().NotNull();
                lifetimeManager.Start();

                var window = container.Resolve<IApplicationEntryPointWindow>().NotNull();
                window.Show();
            }
            catch (Exception) when (!Debugger.IsAttached)
            {
                Environment.ExitCode = 1;
                throw;
            }
        }
    }
}