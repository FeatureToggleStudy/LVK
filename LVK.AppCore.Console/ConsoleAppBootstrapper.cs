using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        [NotNull]
        public static async Task<int> RunAsync<T>([NotNull] string[] args)
            where T: class, IServicesBootstrapper, new()
        {
            var container = new Container();
            container.Bootstrap<ServicesBootstrapper>();
            container.Bootstrap<T>();
            container.UseInstance(args);

            var initializers = container.Resolve<IEnumerable<IApplicationInitialization>>();
            var cleaners = container.Resolve<IEnumerable<IApplicationCleanup>>();

            try
            {
                if (initializers != null)
                {
                    using (var startupCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
                    {
                        try
                        {
                            foreach (IApplicationInitialization initializer in initializers)
                                await initializer.Initialize(startupCts.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            System.Console.Error.WriteLine("application took to long to initialize, terminating");
                            return 1;
                        }
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
                if (cleaners != null)
                {
                    using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(40)))
                    {
                        try
                        {
                            foreach (var cleaner in cleaners)
                                await cleaner.Cleanup(timeoutCts.Token);
                        }
                        catch (TaskCanceledException)
                        {
                            System.Console.Error.WriteLine(
                                "application took to long to terminate gracefully, terminating abornmaly");
                        }
                    }
                }
            }
        }
    }
}