using DryIoc;

using LVK.AppCore.Windows.Wpf;
using LVK.DryIoc;

namespace XamlSandbox
{
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Configuration.ServicesBootstrapper>();
            container.Bootstrap<LVK.Core.Services.ServicesBootstrapper>();
            container.Bootstrap<LVK.Mvvm.ServicesBootstrapper>();

            container.Register<IApplicationEntryPointWindow, MainWindow>();
        }
    }
}