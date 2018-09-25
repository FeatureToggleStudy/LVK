using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore
{
    public class AssemblyLoadMonitorBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly ILogger _Logger;

        public AssemblyLoadMonitorBackgroundService([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Execute(CancellationToken cancellationToken)
        {
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomainOnAssemblyLoad;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
            try
            {
                await cancellationToken.AsTask();
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainOnAssemblyResolve;
                AppDomain.CurrentDomain.AssemblyLoad -= CurrentDomainOnAssemblyLoad;
            }
        }

        private Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            _Logger.LogDebug($"assembly-resolution failed: {args.Name}");
            return null;
        }

        private void CurrentDomainOnAssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            _Logger.LogDebug($"assembly-load: {args.LoadedAssembly.FullName}");
        }
    }
}