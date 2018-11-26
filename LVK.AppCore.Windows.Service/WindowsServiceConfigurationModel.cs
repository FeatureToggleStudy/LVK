using System.Diagnostics;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service
{
    [UsedImplicitly]
    internal class WindowsServiceConfigurationModel
    {
        [NotNull]
        private string _Name = GetDefaultName();

        [NotNull]
        public string Name
        {
            get => _Name;
            set => _Name = string.IsNullOrWhiteSpace(value) ? GetDefaultName() : value;
        }

        [NotNull]
        private static string GetDefaultName()
        {
            using (var currentProcess = Process.GetCurrentProcess())
                return currentProcess.ProcessName;
        }
    }
}