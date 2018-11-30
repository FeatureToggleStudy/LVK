using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Core.Services;

namespace LVK.AppCore.Console
{
    internal class ConsoleApplicationHelpTextPresenter : IConsoleApplicationHelpTextPresenter
    {
        [NotNull, ItemNotNull]
        private readonly IEnumerable<IOptionsHelpTextProvider> _OptionsHelpTextProviders;

        public ConsoleApplicationHelpTextPresenter([NotNull, ItemNotNull] IEnumerable<IOptionsHelpTextProvider> optionsHelpTextProviders)
        {
            _OptionsHelpTextProviders = optionsHelpTextProviders ?? throw new ArgumentNullException(nameof(optionsHelpTextProviders));
        }

        public void Present()
        {
            Assembly assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            
            var command = Path.GetFileNameWithoutExtension(assembly.Location);
            System.Console.WriteLine($"help: {command.ToLowerInvariant()} [commands/arguments] --key[=value]");
            System.Console.WriteLine("notes:");
            System.Console.WriteLine("  * if =value is omitted, a value of 'true' is substituted");
            System.Console.WriteLine(
                "  * configuration can be stored in appsettings.json as json. Paths denoted below are paths into similarly configured json files.");

            System.Console.WriteLine(
                "  * to read additional configuration files, use '@filename.json' syntax as a separate command line argument.");

            var helpTexts =
                from optionsHelpTextProvider in _OptionsHelpTextProviders
                from optionsHelpText in optionsHelpTextProvider.GetHelpText()
                let paths = optionsHelpText.paths.NotNull().ToList()
                where paths != null
                where paths.NotNull().Count > 0
                orderby paths.NotNull().First()
                select optionsHelpText;
            
            foreach ((IEnumerable<string> paths, bool isConfiguration, string description) optionsHelpText in helpTexts)
            {
                System.Console.WriteLine();

                var paths = optionsHelpText.paths.ToList();
                if (paths.Count == 1)
                {
                    System.Console.WriteLine(optionsHelpText.isConfiguration ? $"--{paths[0]}=" : paths[0]);
                }
                else
                {
                    System.Console.WriteLine("paths:");

                    foreach (var path in paths)
                        System.Console.WriteLine($"  {(optionsHelpText.isConfiguration ? "--" : "")}{path}");
                }

                if (paths.Count > 1)
                    System.Console.WriteLine("description:");
                using (var reader = new StringReader(optionsHelpText.description))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        System.Console.WriteLine($"  {line}");
                }
            }

        }
    }
}