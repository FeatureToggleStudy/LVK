using System;

namespace LVK.Logging
{
    internal class DummyLogger<T> : ILogger<T>
    {
        public void Log(LogLevel level, string message)
        {
            // Do nothing here
        }

        public void WriteLine(string line)
        {
            // Do nothing here
        }
    }
}