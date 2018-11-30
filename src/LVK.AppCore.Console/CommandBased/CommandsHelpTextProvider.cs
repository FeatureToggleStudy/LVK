using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Core.Services;

namespace LVK.AppCore.Console.CommandBased
{
    internal class CommandsHelpTextProvider : IOptionsHelpTextProvider
    {
        [NotNull, ItemNotNull]
        private readonly List<IApplicationCommand> _Commands;

        public CommandsHelpTextProvider([NotNull] IEnumerable<IApplicationCommand> commands)
        {
            if (commands is null)
                throw new ArgumentNullException(nameof(commands));

            _Commands = commands.ToList();
        }

        public IEnumerable<(IEnumerable<string> paths, bool isConfiguration, string description)> GetHelpText()
        {
            return from command in _Commands select ((IEnumerable<string>)command.CommandNames, false, command.Description);
        }
    }
}