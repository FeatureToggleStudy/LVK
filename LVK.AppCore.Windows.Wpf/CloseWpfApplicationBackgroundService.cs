using System.Threading;
using System.Threading.Tasks;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.AppCore.Windows.Wpf
{
    internal class CloseWpfApplicationBackgroundService : IBackgroundService
    {
        public async Task Execute(CancellationToken cancellationToken)
        {
            await cancellationToken.AsTask();
            System.Windows.Application.Current.NotNull().Shutdown();
        }
    }
}