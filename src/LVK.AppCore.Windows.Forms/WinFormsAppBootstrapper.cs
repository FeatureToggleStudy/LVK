﻿using System;
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
        public static int RunWinFormsMainWindow<T>(bool useBackgroundServices)
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

                var entryPoint = container.Resolve<IApplicationEntryPoint>().NotNull();
                return entryPoint.Execute(applicationLifetimeManager.GracefulTerminationCancellationToken).GetAwaiter().GetResult();
            }
            catch (Exception) when (!Debugger.IsAttached)
            {
                Environment.ExitCode = 1;
                throw;
            }
            finally
            {
                applicationLifetimeManager.SignalGracefulTermination();

                if (useBackgroundServices)
                    backgroundServicesManager.WaitForBackgroundServicesToStop().GetAwaiter().GetResult();
            }
        }
    }
}
