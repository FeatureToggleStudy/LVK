﻿using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Logging
{
    internal class Logger : ILogger
    {
        [NotNull, ItemNotNull]
        private readonly IEnumerable<ILoggerDestination> _LoggerDestinations;

        public Logger([NotNull, ItemNotNull] IEnumerable<ILoggerDestination> loggerDestinations)
        {
            _LoggerDestinations = loggerDestinations ?? throw new ArgumentNullException(nameof(loggerDestinations));
        }

        public void Log(LogLevel level, string message)
        {
            foreach (ILoggerDestination destination in _LoggerDestinations)
                destination.Log(level, message);
        }

        public void Log(LogLevel level, Func<string> getMessage)
        {
            foreach (ILoggerDestination destination in _LoggerDestinations)
                destination.Log(level, getMessage);
        }

        public void WriteLine(string line)
        {
            foreach (ILoggerDestination destination in _LoggerDestinations)
                destination.WriteLine(line);
        }

    }

    internal class Logger<T> : ILogger<T>
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull]
        private readonly IConfigurationElement<LoggerSystemOptions> _Options;

        public Logger([NotNull] ILogger logger, [NotNull] IConfiguration configuration)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Options = configuration[$"Logging/Systems/{typeof(T).Name}"]
               .Element<LoggerSystemOptions>()
               .WithDefault(() => new LoggerSystemOptions());
        }

        public void Log(LogLevel level, string message)
        {
            if (_Options.Value().Enabled)
                _Logger.Log(level, message);
        }

        public void Log(LogLevel level, Func<string> getMessage)
        {
            if (_Options.Value().Enabled)
                _Logger.Log(level, getMessage);
        }

        public void WriteLine(string line)
        {
            if (_Options.Value().Enabled)
                _Logger.WriteLine(line);
        }
    }
}