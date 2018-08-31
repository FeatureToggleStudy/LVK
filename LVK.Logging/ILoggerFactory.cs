﻿using JetBrains.Annotations;

namespace LVK.Logging
{
    public interface ILoggerFactory
    {
        [NotNull]
        ILogger CreateLogger([NotNull] string systemName);
    }
}