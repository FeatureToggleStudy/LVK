using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Net.Http.Server.Configuration;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

using static LVK.Core.JetBrainsHelpers;

using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace LVK.Net.Http.Server
{
    internal class WebServer : IWebServer
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly IApplicationContext _ApplicationContext;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly SemaphoreSlim _Lock = new SemaphoreSlim(1, 1);

        private bool _HasStopped;
        private Task _Task;
        private CancellationTokenSource _WebServerCancellationTokenSource;

        [NotNull, ItemNotNull]
        private readonly List<IConfigurationConfigurator> _ConfigurationConfigurators;

        public WebServer(
            [NotNull] IContainer container, [NotNull] IApplicationContext applicationContext,
            [NotNull, ItemNotNull] IEnumerable<IConfigurationConfigurator> configurationConfigurators,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            if (configurationConfigurators == null)
                throw new ArgumentNullException(nameof(configurationConfigurators));

            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _ApplicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _ConfigurationConfigurators = configurationConfigurators.ToList();
        }

        public async Task StartAsync()
        {
            await Task.Yield();
            await _Lock.WaitAsync().NotNull();
            try
            {
                if (IsRunning)
                    return;

                if (_HasStopped)
                    throw new InvalidOperationException("Cannot restart web server, please restart application");
                
                _WebServerCancellationTokenSource = new CancellationTokenSource();
                _Task = Task.Run(() => RunWebServerAsync(_WebServerCancellationTokenSource.Token));
            }
            finally
            {
                _Lock.Release();
            }
        }

        public bool IsRunning => _Task != null;

        public async Task StopAsync()
        {
            await Task.Yield();
            await _Lock.WaitAsync().NotNull();
            try
            {
                if (!IsRunning)
                    return;

                assume(_WebServerCancellationTokenSource != null && _Task != null);

                _WebServerCancellationTokenSource.Cancel();
                await _Task;
                
                _Task.Dispose();
                _WebServerCancellationTokenSource.Dispose();

                _HasStopped = true;
                _Task = null;
                _WebServerCancellationTokenSource = null;
            }
            finally
            {
                _Lock.Release();
            }
        }

        [NotNull]
        private async Task RunWebServerAsync(CancellationToken cancellationToken)
        {
            WebApiStartup.Container = _Container;

            var webHost = WebHost.CreateDefaultBuilder(_ApplicationContext.CommandLineArguments.ToArray())
               .UseConfiguration(CreateMicrosoftConfiguration())
               .UseStartup<WebApiStartup>()
               .UseKestrel()
               .NotNull()
               .Build();

            using (var cts = CancellationTokenSource.CreateLinkedTokenSource(
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken, cancellationToken))
            {
                cts.Token.Register(() => Console.WriteLine("Shutdown of web server requested"));
                await webHost.RunAsync(cts.Token).NotNull();
            }
        }

        [NotNull]
        private IConfiguration CreateMicrosoftConfiguration()
        {
            var builder = new ConfigurationBuilder().AddCommandLine(_ApplicationContext.CommandLineArguments.ToArray()).NotNull();

            var target = new ConfigurationConfiguratorTarget();
            foreach (var configurator in _ConfigurationConfigurators)
                configurator.Configure(target);

            foreach (var jsonFile in target.JsonFiles)
                builder.AddJsonFile(jsonFile.Filename, jsonFile.IsOptional);

            return builder.Build().NotNull();
        }
    }
}