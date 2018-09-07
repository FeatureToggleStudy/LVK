using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.AppCore.Console.CommandBased
{
    public class CommandBasedApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IConsoleApplicationHelpTextPresenter _HelpTextPresenter;

        [NotNull]
        private readonly List<IApplicationCommand> _Commands;

        public CommandBasedApplicationEntryPoint(
            [NotNull, ItemNotNull] IEnumerable<IApplicationCommand> commands, [NotNull] IConfiguration configuration,
            [NotNull] IConsoleApplicationHelpTextPresenter helpTextPresenter)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands));

            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _HelpTextPresenter = helpTextPresenter ?? throw new ArgumentNullException(nameof(helpTextPresenter));

            _Commands = commands.ToList();
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            var commandName = GetCommandName();
            if (string.IsNullOrWhiteSpace(commandName))
            {
                _HelpTextPresenter.Present();
                return 1;
            }

            foreach (var command in _Commands)
            {
                if (!command.CommandNames.Any(cn => StringComparer.InvariantCultureIgnoreCase.Equals(cn, commandName)))
                    continue;

                var result = await command.TryExecute();
                return result;
            }

            _HelpTextPresenter.Present();
            return 1;
        }

        [CanBeNull]
        private string GetCommandName()
        {
            return (
                from arg in _Configuration["CommandLineArguments"].Value<string[]>()
                where !arg.StartsWith("-")
                select arg).FirstOrDefault();
        }
    }
}