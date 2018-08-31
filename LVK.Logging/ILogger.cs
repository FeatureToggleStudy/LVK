﻿using JetBrains.Annotations;

namespace LVK.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, [NotNull] string message);
    }
}