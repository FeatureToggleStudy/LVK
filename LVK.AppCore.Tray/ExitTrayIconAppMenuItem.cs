using System;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Tray
{
    internal class ExitTrayIconAppMenuItem : ITrayIconMenuItem
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        public ExitTrayIconAppMenuItem([NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public int Order => int.MaxValue;

        public bool BeginGroup => true;

        public string Caption => "Exit application";

        public bool Visible => true;

        public bool Enabled => true;

        public void Execute()
        {
            _Logger.LogInformation("User selected to exit application, terminating gracefully");
            _ApplicationLifetimeManager.SignalGracefulTermination();
        }
    }
}