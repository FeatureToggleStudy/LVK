using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Core.Services;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues.FileBased
{
    [PublicAPI]
    public class FileWorkQueueRepository : IWorkQueueRepository
    {
        [NotNull]
        private readonly IBus _Bus;

        [NotNull]
        private readonly IConfigurationElementWithDefault<FileWorkQueueConfiguration> _Configuration;

        public FileWorkQueueRepository([NotNull] IConfiguration configuration, [NotNull] IBus bus)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _Bus = bus ?? throw new ArgumentNullException(nameof(bus));

            _Configuration = configuration.Element<FileWorkQueueConfiguration>("WorkQueues/FileBased").WithDefault(() => new FileWorkQueueConfiguration());
        }

        public void Enqueue(string type, JObject payload, DateTime whenToProcess, int retryCount)
        {
            if (!IsEnabled)
                return;

            var model = new WorkQueueModel { Type = type, Payload = payload, WhenToProcess = whenToProcess, RetryCount = retryCount };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented).NotNull();
            var hash = Hash(json);

            var filename = Path.Combine(_Configuration.Value().Path, hash + ".json");
            if (File.Exists(filename))
                return;

            File.WriteAllText(filename, json, Encoding.UTF8);
            File.SetCreationTime(filename, whenToProcess);
            File.SetLastWriteTime(filename, whenToProcess);

            _Bus.Publish(new WorkQueueItemAddedMessage());
        }

        public void Faulted(string type, JObject payload)
        {
            if (!IsEnabled)
                return;

            var model = new WorkQueueFaultedModel { Type = type, Payload = payload };
            var json = JsonConvert.SerializeObject(model, Formatting.Indented).NotNull();
            var hash = Hash(json);

            var filename = Path.Combine(_Configuration.Value().Path, hash + ".faulted");
            if (!File.Exists(filename))
                File.WriteAllText(filename, json, Encoding.UTF8);
        }

        [NotNull]
        private string Hash([NotNull] string json)
        {
            using (SHA1 sha = SHA1.Create().NotNull())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
                return BitConverter.ToString(hashBytes).Replace("-", "");
            }
        }

        public IWorkQueueModel Dequeue()
        {
            if (!IsEnabled)
                return null;

            IEnumerable<string> itemFiles =
                from file in new DirectoryInfo(_Configuration.Value().Path).GetFiles("*.json")
                orderby file.CreationTime
                select file.FullName;

            var firstItem = itemFiles.FirstOrDefault();
            if (firstItem == null)
                return null;

            var json = File.ReadAllText(firstItem, Encoding.UTF8);
            var model = JsonConvert.DeserializeObject<WorkQueueModel>(json);
            File.Delete(firstItem);
            return model;
        }

        public bool IsEnabled
        {
            get
            {
                if (!_Configuration.Value().IsEnabled)
                    return false;

                return !String.IsNullOrWhiteSpace(_Configuration.Value().Path);
            }
        }
    }
}