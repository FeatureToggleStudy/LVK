using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Console.CommandBased
{
    internal class CommandBasedApplicationEntryPoint : ConsoleApplicationEntryPointBase, IApplicationEntryPoint
    {
        [NotNull]
        private readonly IConfiguration _Configuration;

        [NotNull]
        private readonly IConsoleApplicationHelpTextPresenter _HelpTextPresenter;

        [NotNull]
        private readonly List<IApplicationCommand> _Commands;

        public CommandBasedApplicationEntryPoint(
            [NotNull, ItemNotNull] IEnumerable<IApplicationCommand> commands, [NotNull] IConfiguration configuration,
            [NotNull] IConsoleApplicationHelpTextPresenter helpTextPresenter,
            [NotNull] ILogger logger, [NotNull] IBus bus, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
            : base(logger, bus, applicationLifetimeManager)
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

            HookCtrlC();

            foreach (var command in _Commands)
            {
                if (!command.CommandNames.Any(cn => StringComparer.InvariantCultureIgnoreCase.Equals(cn, commandName)))
                    continue;

                var result = await command.TryExecute(GetParameters().ToArray());
                return result;
            }

            _HelpTextPresenter.Present();
            return 1;
        }

        [NotNull, ItemNotNull]
        private IEnumerable<string> GetCommandLineArguments() => _Configuration["CommandLineArguments"].Element<string[]>().WithDefault(new string[0]).Value();

        private bool IsCommandArgument([NotNull] string arg) => !arg.StartsWith("-");

        [CanBeNull]
        private string GetCommandName()
        {
            return (
                from arg in GetCommandLineArguments()
                where IsCommandArgument(arg)
                select arg).FirstOrDefault();
        }

        [NotNull, ItemNotNull]
        private List<string> GetParameters()
        {
            var result = new List<string>();
            bool commandFound = false;
            foreach (string arg in GetCommandLineArguments())
            {
                if (!commandFound && IsCommandArgument(arg))
                    commandFound = true;
                else
                    result.Add(arg);
            }

            return result;
        }
    }
}