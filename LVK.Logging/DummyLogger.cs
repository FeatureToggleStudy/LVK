namespace LVK.Logging
{
    internal class DummyLogger : ILogger
    {
        public void Log(LogLevel level, string message)
        {
            // Do nothing here
        }
    }
}