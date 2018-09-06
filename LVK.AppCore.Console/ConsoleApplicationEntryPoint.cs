using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationEntryPoint : IConsoleApplicationEntryPoint
    {
        [NotNull]
        private readonly IApplicationEntryPoint _ApplicationEntryPoint;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull, ItemNotNull]
        private readonly List<IOptionsHelpTextProvider> _OptionsHelpTextProviders;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly List<IBackgroundService> _BackgroundServices;

        private bool _WasCancelledByUser;

        public ConsoleApplicationEntryPoint(
            [NotNull] IApplicationEntryPoint applicationEntryPoint, [NotNull] ILogger logger,
            [NotNull] IEnumerable<IBackgroundService> backgroundServices,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] IConfiguration configuration,
            [NotNull, ItemNotNull] IEnumerable<IOptionsHelpTextProvider> optionsHelpTextProviders)
        {
            if (backgroundServices == null)
                throw new ArgumentNullException(nameof(backgroundServices));

            if (optionsHelpTextProviders == null)
                throw new ArgumentNullException(nameof(optionsHelpTextProviders));

            _ApplicationEntryPoint =
                applicationEntryPoint ?? throw new ArgumentNullException(nameof(applicationEntryPoint));

            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _OptionsHelpTextProviders = optionsHelpTextProviders.ToList();

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _BackgroundServices = backgroundServices.ToList();
        }

        public async Task<int> RunAsync()
        {
            var userCancelKeyPressCancellationTokenSource = new CancellationTokenSource();
            var cts = CancellationTokenSource.CreateLinkedTokenSource(
                userCancelKeyPressCancellationTokenSource.Token,
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken);

            System.Console.CancelKeyPress += (s, e) =>
            {
                _Logger.Log(LogLevel.Debug, "User cancelled application with Ctrl+C");
                _WasCancelledByUser = true;
                if (e != null)
                    e.Cancel = true;

                userCancelKeyPressCancellationTokenSource.Cancel();
            };

            using (_Logger.LogScope(LogLevel.Trace, $"{nameof(ConsoleApplicationEntryPoint)}.{nameof(RunAsync)}"))
            {
                if (ShowHelp())
                    return 0;

                return await RunApplicationEntryPoint(cts);
            }
        }

        private bool ShowHelp()
        {
            if (!_Configuration["help"].Value<bool>())
                return false;

            var command = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            System.Console.WriteLine($"help: {command.ToLowerInvariant()} --key[=value]");
            System.Console.WriteLine("notes:");
            System.Console.WriteLine("  * if =value is omitted, a value of 'true' is substituted");
            System.Console.WriteLine(
                "  * configuration can be stored in appsettings.json as json. Paths denoted below are paths into similarly configured json files.");

            System.Console.WriteLine(
                "  * to read additional configuration files, use '@filename.json' syntax as a separate command line argument.");

            var helpTexts =
                from optionsHelpTextProvider in _OptionsHelpTextProviders
                from optionsHelpText in optionsHelpTextProvider.GetHelpText()
                let paths = optionsHelpText.paths.ToList()
                where paths.Count > 0
                orderby paths.First()
                select optionsHelpText;
            
            foreach (var optionsHelpText in helpTexts)
            {
                System.Console.WriteLine();

                var paths = optionsHelpText.paths.ToList();
                if (paths.Count == 1)
                    System.Console.WriteLine(paths[0]);
                else
                {
                    System.Console.WriteLine("paths:");

                    foreach (var path in paths)
                        System.Console.WriteLine($"  {path}");
                }

                if (paths.Count > 1)
                    System.Console.WriteLine("description:");
                using (var reader = new StringReader(optionsHelpText.description))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        System.Console.WriteLine($"  {line}");
                }
            }

            return true;
        }

        [NotNull]
        private async Task<int> RunApplicationEntryPoint(CancellationTokenSource cts)
        {
            try
            {
                if (!await StartBackgroundServices())
                    return 1;

                int exitcode;
                try
                {
                    exitcode = await _ApplicationEntryPoint.Execute(cts.Token);
                    _ApplicationLifetimeManager.SignalGracefulTermination();
                }
                finally
                {
                    if (!await StopBackgroundServices())
                        exitcode = 1;
                }

                return exitcode;
            }
            catch (TaskCanceledException)
            {
                if (!_WasCancelledByUser
                 && !_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
                    System.Console.Error.WriteLine("program terminated early, unknown reason");

                return 1;
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                _Logger.LogException(ex);
                throw;
            }
        }

        private async Task<bool> StartBackgroundServices()
        {
            using (var startupCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    foreach (IBackgroundService arc in _BackgroundServices)
                        using (_Logger.LogScope(LogLevel.Debug, $"starting runtime context {arc.GetType().Name}"))
                            await arc.Start(startupCts.Token);

                    return true;
                }
                catch (TaskCanceledException)
                {
                    System.Console.Error.WriteLine("application took to long to start, terminating");
                    return false;
                }
            }
        }

        private async Task<bool> StopBackgroundServices()
        {
            using (var stopCts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                try
                {
                    List<IBackgroundService> arcs = _BackgroundServices.ToList();
                    arcs.Reverse();

                    foreach (IBackgroundService arc in arcs)
                        using (_Logger.LogScope(LogLevel.Debug, $"stopping runtime context {arc.GetType().Name}"))
                            try
                            {
                                await arc.Stop(stopCts.Token);
                            }
                            catch (TaskCanceledException)
                            {
                            }

                    return true;
                }
                catch (TaskCanceledException)
                {
                    System.Console.Error.WriteLine("application took to long to terminate gracefully, terminating");
                    return false;
                }
            }
        }
    }
}