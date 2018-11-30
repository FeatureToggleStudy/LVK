using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore
{
    [PublicAPI]
    public interface IBackgroundServicesManager
    {
        void StartBackgroundServices();
        
        [NotNull]
        Task WaitForBackgroundServicesToStop();
    }
}