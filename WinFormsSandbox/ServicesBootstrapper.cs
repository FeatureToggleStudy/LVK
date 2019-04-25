using DryIoc;

using LVK.AppCore;
using LVK.AppCore.Windows.Forms;
using LVK.DryIoc;

namespace WinFormsSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<IApplicationEntryPoint, WinFormsApplicationEntryPoint<MainForm>>();
        }
    }
}
