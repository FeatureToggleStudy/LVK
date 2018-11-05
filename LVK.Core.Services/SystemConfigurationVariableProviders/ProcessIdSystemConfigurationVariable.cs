using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace LVK.Core.Services.SystemConfigurationVariableProviders
{
    internal class ProcessSystemConfigurationVariablesProvider : ISystemConfigurationVariablesProvider
    {
        public IEnumerable<(string key, Func<string> getValue)> GetSystemConfigurationVariableProviders()
        {
            yield return ("sys.ProcessId", GetProcessId);
            yield return ("sys.ProcessName", GetProcessName);
            yield return ("sys.MachineName", () => Environment.MachineName);
            yield return ("sys.UserName", () => Environment.UserName);
            yield return ("sys.UserDomainName", () => Environment.UserDomainName);
        }

        private string GetProcessName()
        {
            using (var currentProcess = Process.GetCurrentProcess())
                return currentProcess.ProcessName;
        }

        private string GetProcessId()
        {
            using (var currentProcess = Process.GetCurrentProcess())
                return currentProcess.Id.ToString(CultureInfo.InvariantCulture);
        }
    }
}