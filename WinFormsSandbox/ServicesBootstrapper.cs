using System.Windows.Forms;

using DryIoc;

using LVK.DryIoc;

namespace WinFormsSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.Logging.ServicesBootstrapper>();

            container.Register<Form, MainForm>(serviceKey: "MainForm");
        }
    }
}
