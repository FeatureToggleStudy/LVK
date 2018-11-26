using System.Configuration.Install;
using System.Reflection;

using LVK.Core;

namespace LVK.AppCore.Windows.Service
{
    internal class InstallContextProvider : IInstallContextProvider
    {
        public InstallContext GetContext()
        {
            var context = new InstallContext();
            var executableFilePath = Assembly.GetEntryAssembly().NotNull().Location;
            context.Parameters.NotNull()["assemblypath"] = $"\"{executableFilePath}\" runAsService";
            return context;
        }
    }
}