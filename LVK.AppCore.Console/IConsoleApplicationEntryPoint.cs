using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.AppCore.Console
{
    internal interface IConsoleApplicationEntryPoint
    {
        [NotNull]
        Task<int> RunAsync();
    }
}