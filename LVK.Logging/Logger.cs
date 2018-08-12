using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Logging
{
    internal class Logger : ILogger
    {
        [NotNull]
        private readonly List<ILogDestination> _Destinations;

        public Logger([NotNull] IEnumerable<ILogDestination> destinations)
        {
            _Destinations = (destinations ?? throw new ArgumentNullException(nameof(destinations))).ToList();
        }

        public void Log(LogLevel level, string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            foreach (var destination in _Destinations)
                destination.Log(level, message);
        }
    }
}