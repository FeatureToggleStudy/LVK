using System.Configuration.Install;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service
{
    internal interface IInstallContextProvider
    {
        [NotNull] InstallContext GetContext();
    }
}