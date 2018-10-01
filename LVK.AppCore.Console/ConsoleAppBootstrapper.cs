using System;
using System.Diagnostics;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.AppCore.Console.CommandBased;
using LVK.AppCore.Console.Daemons;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;
using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [PublicAPI]
    public static class ConsoleAppBootstrapper
    {
        [NotNull]
        public static Task<int> RunCommandAsync<T>()
            where T: class, IServicesBootstrapper
            => RunAsync<CommandBasedServicesBootstrapper<T>>();
        
        [NotNull]
        public static Task<int> RunDaemonAsync<T>()
            where T: class, IServicesBootstrapper
            => RunAsync<DaemonServicesBootstrapper<T>>();

        [NotNull]
        public static async Task<int> RunAsync<T>()
            where T: class, IServicesBootstrapper
        {
            IConsoleApplicationEntryPoint entryPoint;
            try
            {
                var container = ContainerFactory.Create();
                container.Bootstrap<ServicesBootstrapper>();
                container.Bootstrap<T>();

                entryPoint = container.Resolve<IConsoleApplicationEntryPoint>();
                assume(entryPoint != null);
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                OutputExceptionToConsole(ex);
                return 1;
            }

            try
            {
                return await entryPoint.RunAsync().NotNull();
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