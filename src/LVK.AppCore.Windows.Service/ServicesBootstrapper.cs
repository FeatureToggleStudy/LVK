using DryIoc;

using LVK.AppCore.Windows.Service.Commands;
using LVK.AppCore.Windows.Service.Configuration;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Service
{
    internal class ServicesBootstrapper<T> : IServicesBootstrapper
        where T: class, IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<T>();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<Console.ServicesBootstrapper>();
            container.Bootstrap<Core.Services.ServicesBootstrapper>();
            container.Bootstrap<Logging.ServicesBootstrapper>();

            container.Register<IApplicationCommand, InstallWindowsServiceCommand>();
            container.Register<IApplicationCommand, UninstallWindowsServiceCommand>();
            container.Register<IApplicationCommand, RunAsServiceCommand>();
            container.Register<IApplicationCommand, StartWindowsServiceCommand>();
            container.Register<IApplicationCommand, StopWindowsServiceCommand>();
            container.Register<IApplicationCommand, QueryWindowsServiceCommand>();
            container.Register<IApplicationCommand, RunCommand>();
            container.Register<IOptionsHelpTextProvider, WindowsServiceOptionsHelpTextProvider>();
            
            container.Register<IWindowsServiceConfiguration, WindowsServiceConfiguration>();
            container.Register<IInstallContextProvider, InstallContextProvider>();
            container.Register<IPersistentInstallState, PersistentInstallState>();
            container.Register<IWindowsServiceController, WindowsServiceController>();
        }
    }
}