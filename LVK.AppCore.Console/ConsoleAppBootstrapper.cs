using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using static LVK.Core.JetBrainsHelpers;

using LVK.Core;
using LVK.Core.Services;
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
            IContainer container;
            List<IApplicationInitialization> initializers;
            List<IApplicationCleanup> cleaners;
            try
            {
                var containerBuilder = new ContainerBuilder();
                containerBuilder.Register<ServicesRegistrant>();
                containerBuilder.Register<T>();

                container = containerBuilder.Build();
                container.UseInstance(args);

                initializers = container.Resolve<IEnumerable<IApplicationInitialization>>()?.ToList()
                            ?? new List<IApplicationInitialization>();

                cleaners = container.Resolve<IEnumerable<IApplicationCleanup>>()?.ToList()
                        ?? new List<IApplicationCleanup>();
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                OutputExceptionToConsole(ex);
                return 1;
            }

            try
            {
                using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(40)))
                {
                    try
                    {
                        foreach (IApplicationInitialization initializer in initializers)
                            await initializer.Initialize(timeoutCts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        System.Console.Error.WriteLine("application took to long to initialize, terminating abornmaly");
                    }
                }

                return await container.Resolve<IConsoleApplicationEntryPoint>().NotNull().RunAsync().NotNull();
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
            finally
            {
                using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(40)))
                {
                    try
                    {
                        foreach (IApplicationCleanup cleanup in cleaners)
                            await cleanup.Cleanup(timeoutCts.Token);
                    }
                    catch (TaskCanceledException)
                    {
                        System.Console.Error.WriteLine("application took to long to clean up, terminating abornmaly");
                    }
                }
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