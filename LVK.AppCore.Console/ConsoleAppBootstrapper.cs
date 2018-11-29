using System;
using System.Diagnostics;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore.Console.CommandBased;
using LVK.AppCore.Console.Daemons;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;
using LVK.Core.Services;
using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [PublicAPI]
    public static class ConsoleAppBootstrapper
    {
        [NotNull]
        public static Task<int> RunCommandAsync<T>()
            where T: class, IServicesBootstrapper
            => RunAsync<CommandBasedServicesBootstrapper<T>>(false);
        
        [NotNull]
        public static Task<int> RunDaemonAsync<T>()
            where T: class, IServicesBootstrapper
            => RunAsync<DaemonServicesBootstrapper<T>>(true);

        [NotNull]
        public static async Task<int> RunAsync<T>(bool useBackgroundServices)
            where T: class, IServicesBootstrapper
        {
            IConsoleApplicationEntryPoint entryPoint;
            IBackgroundServicesManager backgroundServicesManager;
            IApplicationLifetimeManager applicationLifetimeManager;
            try
            {
                var container = ContainerFactory.Bootstrap<ServicesBootstrapper, T>();

                backgroundServicesManager = container.Resolve<IBackgroundServicesManager>().NotNull();
                entryPoint = container.Resolve<IConsoleApplicationEntryPoint>().NotNull();
                applicationLifetimeManager = container.Resolve<IApplicationLifetimeManager>().NotNull();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                OutputExceptionToConsole(ex);
                return 1;
            }

            try
            {
                if (useBackgroundServices)
                    backgroundServicesManager.StartBackgroundServices();
                try
                {
                    return await entryPoint.RunAsync().NotNull();
                }
                finally
                {
                    applicationLifetimeManager.SignalGracefulTermination();

                    if (useBackgroundServices)
                        await backgroundServicesManager.WaitForBackgroundServicesToStop();
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                OutputExceptionToConsole(ex);
                return 1;
            }
        }

        private static void OutputExceptionToConsole(Exception ex)
        {
            while (ex != null)
            {
                Type exType = ex.GetType();
                assume(exType != null);

                System.Console.Error.WriteLine($"{exType.Name}: {ex.Message}");
                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                    System.Console.Error.WriteLine(ex.StackTrace);

                ex = ex.InnerException;
                if (ex != null)
                    System.Console.Error.WriteLine();
            }
        }
    }
}