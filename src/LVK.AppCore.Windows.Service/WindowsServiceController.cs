using System;
using System.ServiceProcess;

using JetBrains.Annotations;

using LVK.Logging;

namespace LVK.AppCore.Windows.Service
{
    internal class WindowsServiceController : IWindowsServiceController
    {
        [NotNull]
        private readonly IPersistentInstallState _PersistentInstallState;

        [NotNull]
        private readonly ILogger _Logger;

        public WindowsServiceController([NotNull] IPersistentInstallState persistentInstallState, [NotNull] ILogger logger)
        {
            _PersistentInstallState = persistentInstallState;
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [CanBeNull]
        private ServiceController CreateServiceController()
        {
            var state = _PersistentInstallState.Load();
            string serviceName = state[StateConstants.InstalledServiceName] as string; 
            if (serviceName is null)
                return null;

            return CreateServiceController(serviceName);
        }

        [CanBeNull]
        private ServiceController CreateServiceController([NotNull] string serviceName)
        {
            var serviceController = new ServiceController(serviceName);
            try
            {
                GC.KeepAlive(serviceController.Status);
                return serviceController;
            }
            catch (InvalidOperationException)
            {
                serviceController.Dispose();
                return null;
            }

            // List<ServiceController> services = null;
            // try
            // {
            //     services = ServiceController.GetServices().ToList();
            //
            //     var myService =
            //         services.FirstOrDefault(srv => StringComparer.InvariantCultureIgnoreCase.Equals(srv?.ServiceName, serviceName));
            //
            //     if (myService != null)
            //         services.Remove(myService);
            //
            //     return myService;
            // }
            // finally
            // {
            //     if (services != null)
            //         foreach (var srv in services)
            //             srv.Dispose();
            // }
        }

        public bool IsInstalled()
        {
            using (var controller = CreateServiceController())
                return controller != null;
        }

        public bool IsRunning()
        {
            using (var controller = CreateServiceController())
            {
                if (controller is null)
                    return false;

                return GetIsRunningStatus(controller);
            }
        }

        private static bool GetIsRunningStatus([NotNull] ServiceController controller)
        {
            switch (controller.Status)
            {
                case ServiceControllerStatus.Running:
                case ServiceControllerStatus.StartPending:
                case ServiceControllerStatus.StopPending:
                case ServiceControllerStatus.ContinuePending:
                case ServiceControllerStatus.PausePending:
                    return true;

                case ServiceControllerStatus.Paused:
                case ServiceControllerStatus.Stopped:
                    return false;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Start()
        {
            using (var controller = CreateServiceController())
            {
                if (controller is null)
                {
                    _Logger.LogError("Unable to start service because it has not been installed");
                    return;
                }

                if (GetIsRunningStatus(controller))
                {
                    _Logger.LogError("Service is already running");
                    return;
                }

                using (_Logger.LogScope(LogLevel.Debug, $"Attempting to start service {controller.ServiceName}"))
                {
                    _Logger.LogInformation($"Attempting to start service {controller.ServiceName}");
                    controller.Start();
                    _Logger.LogInformation($"Service {controller.ServiceName} successfully started");
                }
            }
        }

        public void Stop()
        {
            using (var controller = CreateServiceController())
            {
                if (controller is null)
                {
                    _Logger.LogError("Unable to stop service because it has not been installed");
                    return;
                }

                if (!GetIsRunningStatus(controller))
                {
                    _Logger.LogError("Service is not running");
                    return;
                }

                using (_Logger.LogScope(LogLevel.Debug, $"Attempting to stop service {controller.ServiceName}"))
                {
                    _Logger.LogInformation($"Attempting to stop service {controller.ServiceName}");
                    controller.Stop();
                    _Logger.LogInformation($"Service {controller.ServiceName} successfully stopped");
                }
            }
        }

        public void QueryStatus()
        {
            using (var controller = CreateServiceController())
            {
                if (controller is null)
                {
                    _Logger.LogInformation("Service has not been installed");
                    return;
                }

                _Logger.LogInformation($"Service '{controller.ServiceName}': {controller.Status}");
            }
        }
    }
}