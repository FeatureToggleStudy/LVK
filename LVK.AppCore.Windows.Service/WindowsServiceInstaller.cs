using System.Configuration.Install;
using System.ServiceProcess;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service
{
    internal class WindowsServiceInstaller : Installer 
    {
        public WindowsServiceInstaller([NotNull] string serviceName)
        {
            var processInstaller = new ServiceProcessInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;
            
            var serviceInstaller = new ServiceInstaller();
            serviceInstaller.ServiceName = serviceName;
            serviceInstaller.DisplayName = serviceName;
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            Installers.Add(serviceInstaller);
            Installers.Add(processInstaller);
        }
    }
}