using System;
using System.Diagnostics;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;
using LVK.DryIoc;

namespace LVK.AppCore.Console
{
    [PublicAPI]
    public class ConsoleAppBootstrapper
    {
        public static Task<int> RunDaemonAsync<T>([NotNull] string[] args)
            where T: class, IServicesRegistrant, new()
            => RunAsync<DaemonServicesRegistrant<T>>(args);

        [NotNull]
        public static async Task<int> RunAsync<T>([NotNull] string[] args)
            where T: class, IServicesRegistrant, new()
        {
            IConsoleApplicationEntryPoint entryPoint;
            try
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.Register<ServicesRegistrant>();
                containerBuilder.Register<T>();

                entryPoint = containerBuilder.Build().Resolve<IConsoleApplicationEntryPoint>();
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