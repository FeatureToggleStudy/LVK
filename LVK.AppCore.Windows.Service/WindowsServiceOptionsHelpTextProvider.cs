using System.Collections.Generic;

using LVK.Core.Services;

namespace LVK.AppCore.Windows.Service
{
    internal class WindowsServiceOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText()
        {
            yield return (new[] { "WindowsService/Name" }, true, @"Name of Windows Service
Defaults to name of executable

example:
  --WindowsService/Name=MyAwesomeWindowsService

example appsettings.json:
  {
    ""WindowsService"": {
      ""Name"": ""MyAwesomeWindowsService""
      }
    }
  }");

        }
    }
}