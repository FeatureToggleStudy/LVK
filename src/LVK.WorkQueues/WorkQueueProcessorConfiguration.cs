using Newtonsoft.Json;

namespace LVK.WorkQueues
{
    internal class WorkQueueProcessorConfiguration
    {
        [JsonProperty("Workers")]
        public int WorkerThreads { get; set; } = 4;
    }
}