using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Commands;
using LVK.Core.Services;

namespace LVK.AppCore.Commands
{
    internal class StopApplicationCommandHandler : ICommandHandler<StopApplicationCommand>
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public StopApplicationCommandHandler([NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public Task<bool> Handle(StopApplicationCommand input)
        {
            _ApplicationLifetimeManager.SignalGracefulTermination();
            return Task.FromResult(true);
        }
    }
}