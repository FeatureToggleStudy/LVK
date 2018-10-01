using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DryIoc;

using JetBrains.Annotations;

using LVK.Logging;
using LVK.Reflection;

namespace LVK.Commands
{
    internal class CommandDispatcher : ICommandDispatcher
    {
        [NotNull]
        private readonly IContainer _Container;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        public CommandDispatcher([NotNull] IContainer container, [NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper)
        {
            _Container = container ?? throw new ArgumentNullException(nameof(container));
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
        }

        public async Task<(bool success, TOutput output)> TryDispatch<TInput, TOutput>(TInput input)
        {
            using (_Logger.LogScope(LogLevel.Trace, nameof(CommandDispatcher) + "." + nameof(TryDispatch)))
            {
                var handlers = _Container.Resolve<IEnumerable<ICommandHandler<TInput, TOutput>>>()
                   .ToList();
                if (!handlers.Any())
                {
                    _Logger.LogDebug($"no command handler for '{_TypeHelper.NameOf<TInput>()} -> {_TypeHelper.NameOf<TOutput>()}'");
                    return (false, default(TOutput));
                }

                using (_Logger.LogScope(LogLevel.Debug, $"dispatch '{_TypeHelper.NameOf<TInput>()} -> {_TypeHelper.NameOf<TOutput>()}'"))
                {
                    foreach (var handler in handlers)
                    {
                        var (success, output) = await handler.Handle(input);
                        if (success)
                            return (true, output);
                    }

                    return (false, default(TOutput));
                }
            }
        }

        public async Task<bool> TryDispatch<TInput>(TInput input)
        {
            using (_Logger.LogScope(LogLevel.Trace, nameof(CommandDispatcher) + "." + nameof(TryDispatch)))
            {
                var handlers = _Container.Resolve<IEnumerable<ICommandHandler<TInput>>>()
                   .ToList();
                if (!handlers.Any())
                {
                    _Logger.LogDebug($"no command handler for '{_TypeHelper.NameOf<TInput>()}'");
                    return false;
                }

                using (_Logger.LogScope(LogLevel.Debug, $"dispatch '{_TypeHelper.NameOf<TInput>()}'"))
                {
                    foreach (var handler in handlers)
                    {
                        var success = await handler.Handle(input);
                        if (success)
                            return true;
                    }

                    return false;
                }
            }
        }
    }
}