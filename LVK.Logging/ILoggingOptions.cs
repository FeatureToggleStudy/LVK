namespace LVK.Logging
{
    public interface ILoggingOptions
    {
        bool VerboseEnabled { get; set; }

        bool DebugEnabled { get; set; }
    }
}