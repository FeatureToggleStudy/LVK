using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Http.Server
{
    [PublicAPI]
    public interface IWebServer
    {
        [NotNull]
        Task StartAsync();

        bool IsRunning { get; }

        [NotNull]
        Task StopAsync();
    }
}