namespace LVK.AppCore.Windows.Service
{
    internal interface IWindowsServiceController
    {
        bool IsInstalled();
        bool IsRunning();

        void Start();
        void Stop();
        void QueryStatus();
    }
}