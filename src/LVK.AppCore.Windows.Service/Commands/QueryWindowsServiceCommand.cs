using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service.Commands
{
    internal class QueryWindowsServiceCommand : IApplicationCommand
    {
        [NotNull]
        private readonly IWindowsServiceController _WindowsServiceController;

        public QueryWindowsServiceCommand([NotNull] IWindowsServiceController windowsServiceController)
        {
            _WindowsServiceController = windowsServiceController ?? throw new ArgumentNullException(nameof(windowsServiceController));
        }

        public string[] CommandNames => new[] { "query" };

        public string Description => "Query Windows Service Status";

        public Task<int> TryExecute()
        {
            _WindowsServiceController.QueryStatus();
            return Task.FromResult(0);
        }
    }
}