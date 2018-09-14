using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Core;
using LVK.Core.Services;
using LVK.Logging;
using LVK.Persistence;

namespace ConsoleSandbox
{
    internal class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IPersistentData<PersistentModel> _PersistentModel;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public ApplicationEntryPoint(
            [NotNull] ILogger logger,
            [NotNull] IPersistentData<PersistentModel> persistentModel,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _PersistentModel = persistentModel ?? throw new ArgumentNullException(nameof(persistentModel));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            _Logger.Log(LogLevel.Trace, "ApplicationEntryPoint.Execute");

            while (!_ApplicationLifetimeManager.GracefulTerminationCancellationToken.IsCancellationRequested)
            {
                await Task.Delay(500, _ApplicationLifetimeManager.GracefulTerminationCancellationToken).NotNull();
                using (_PersistentModel.UpdateScope())
                    for (int index = 0; index < 100; index++)
                        _PersistentModel.Value.Count++;
                _Logger.Log(LogLevel.Information, $"Counter = {_PersistentModel.Value.Count}");
            }

            return 0;
        }
    }
}