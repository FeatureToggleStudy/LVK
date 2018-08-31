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
            where T: class, IServicesBootstrapper, new()
            => RunAsync<DaemonServicesBootstrapper<T>>(args);

        [NotNull]
        public static async Task<int> RunAsync<T>([NotNull] string[] args)
            where T: class, IServicesBootstrapper, new()
        {
            var container = new Container();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<T>();
            container.UseInstance(args);

            List<IApplicationInitialization> initializers =
                container.Resolve<IEnumerable<IApplicationInitialization>>()?.ToList()
             ?? new List<IApplicationInitialization>();

            List<IApplicationCleanup> cleaners = container.Resolve<IEnumerable<IApplicationCleanup>>()?.ToList()
                                              ?? new List<IApplicationCleanup>();

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
    }
}