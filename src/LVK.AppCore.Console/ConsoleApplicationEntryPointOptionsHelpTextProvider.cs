using System.Collections.Generic;

using LVK.Core.Services;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationEntryPointOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText()
        {
            yield return (new[] { "help" }, true, @"Show this help-text

example:
  --help");
        }
    }
}