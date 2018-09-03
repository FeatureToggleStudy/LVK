using System.Collections.Generic;

namespace LVK.AppCore.Console
{
    public class ConsoleApplicationEntryPointOptionsHelpTextProvider : IOptionsHelpTextProvider
    {
        public IEnumerable<(IEnumerable<string> paths, string description)> GetHelpText()
        {
            yield return (new[] { "help" }, @"Show this help-text

example:
  --help");
        }
    }
}