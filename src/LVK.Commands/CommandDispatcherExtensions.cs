using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Commands
{
    [PublicAPI]
    public static class CommandDispatcherExtensions
    {
        [NotNull, ItemCanBeNull]
        public static async Task<TOutput> TryDispatch<TInput, TOutput>([NotNull] this ICommandDispatcher commandDispatcher, [NotNull] TInput input)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            var (success, output) = await commandDispatcher.TryDispatch<TInput, TOutput>(input);
            if (!success)
                throw new InvalidOperationException($"processing of '{typeof(TInput)} -> {typeof(TOutput)}' failed");

            return output;
        }

        [NotNull]
        public static async Task Dispatch<TInput>([NotNull] this ICommandDispatcher commandDispatcher, [NotNull] TInput input)
        {
            if (commandDispatcher == null)
                throw new ArgumentNullException(nameof(commandDispatcher));

            var success = await commandDispatcher.TryDispatch(input);
            if (!success)
                throw new InvalidOperationException($"processing of '{typeof(TInput)}' failed");
        }
    }
}