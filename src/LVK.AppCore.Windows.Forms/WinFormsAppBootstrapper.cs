using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Windows.Forms
{
    [PublicAPI]
    public static class WinFormsAppBootstrapper
    {
        public static void RunWinFormsMainWindow<T>(bool useBackgroundServices)
            where T: class, IServicesBootstrapper
        {
            var container = ContainerFactory.Bootstrap<ServicesBootstrapper<T>>();

            IBackgroundServicesManager backgroundServicesManager = container.Resolve<IBackgroundServicesManager>().NotNull();
            IApplicationLifetimeManager applicationLifetimeManager = container.Resolve<IApplicationLifetimeManager>();

            if (useBackgroundServices)
                backgroundServicesManager.StartBackgroundServices();

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Form mainForm = container.Resolve<Form>(serviceKey: "MainForm");

                bool closingHasBeenHandled = true;
                applicationLifetimeManager.GracefulTerminationCancellationToken.Register(
                    () =>
                    {
                        if (closingHasBeenHandled)
                            return;

                        closingHasBeenHandled = true;
                        var close = new Action(() => mainForm.Close());
                        if (mainForm.InvokeRequired)
                            mainForm.BeginInvoke(close);
                        else
                            close();
                    });

                mainForm.FormClosed += (s, e) =>
                {
                    closingHasBeenHandled = true;
                    applicationLifetimeManager.SignalGracefulTermination();
                };

                Application.Run(mainForm);
            }
            catch (Exception) when (!Debugger.IsAttached)
            {
                Environment.ExitCode = 1;
                throw;
            }
            finally
            {
                if (useBackgroundServices)
                    backgroundServicesManager.WaitForBackgroundServicesToStop().GetAwaiter().GetResult();
            }
        }
    }
}