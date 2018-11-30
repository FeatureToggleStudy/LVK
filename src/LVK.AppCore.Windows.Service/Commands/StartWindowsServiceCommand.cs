using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class StartWindowsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly IWindowsServiceController _WindowsServiceController;

        public StartWindowsServiceCommand([NotNull] IWindowsServiceController windowsServiceController)
        {
            _WindowsServiceController = windowsServiceController ?? throw new ArgumentNullException(nameof(windowsServiceController));
        }

        public string[] CommandNames => new[] { "start" };

        public string Description => "Start Windows Service";

        public Task<int> TryExecute()
        {
            _WindowsServiceController.Start();
            return Task.FromResult(0);
        }
    }
}