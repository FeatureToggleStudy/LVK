using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core;
using LVK.Security.Cryptography;

using Newtonsoft.Json;

namespace LVK.WorkQueues.FileBased
{
    [PublicAPI]
    public class FileWorkQueueRepository : IWorkQueueRepository
    {
        [NotNull]
        private readonly IHasher _Hasher;

        [NotNull]
        private readonly IConfigurationElementWithDefault<FileWorkQueueConfiguration> _Configuration;

        public FileWorkQueueRepository([NotNull] IConfiguration configuration, [NotNull] IHasher hasher)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _Hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));

            _Configuration = configuration.Element<FileWorkQueueConfiguration>("WorkQueues/FileBased").WithDefault(() => new FileWorkQueueConfiguration());
        }

        // ReSharper disable once AnnotationRedundancyInHierarchy
        [NotNull]
        public Task EnqueueManyAsync(IEnumerable<WorkQueueItem> items)
        {
            if (!IsEnabled)
                return Task.CompletedTask;

            foreach (var item in items)
            {
                string filename = WriteToFile(item, ".json");
                File.SetCreationTime(filename, item.WhenToProcess);
                File.SetLastWriteTime(filename, item.WhenToProcess);
            }

            return Task.CompletedTask;
        }

        // ReSharper disable once AnnotationRedundancyInHierarchy
        [NotNull]
        public Task FaultedAsync(WorkQueueItem item)
        {
            if (!IsEnabled)
                return Task.CompletedTask;

            WriteToFile(new { item.Type, item.Payload }, ".faulted");

            return Task.CompletedTask;
        }

        [NotNull]
        private string WriteToFile([NotNull] object obj, [NotNull] string extension)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented).NotNull();
            var hash = _Hasher.Hash(json);

            var filename = Path.Combine(_Configuration.Value().Path, hash + extension);
            if (!File.Exists(filename))
                File.WriteAllText(filename, json, Encoding.UTF8);

            return filename;
        }

        Task<WorkQueueItem?> IWorkQueueRepository.DequeueAsync()
        {
            if (!IsEnabled)
                return Task.FromResult<WorkQueueItem?>(null);

            IEnumerable<string> itemFiles =
                from file in new DirectoryInfo(_Configuration.Value().Path).GetFiles("*.json")
                orderby file.CreationTime
                select file.FullName;

            var firstItem = itemFiles.FirstOrDefault();
            if (firstItem == null)
                return Task.FromResult<WorkQueueItem?>(null);

            var json = File.ReadAllText(firstItem, Encoding.UTF8);
            var item = JsonConvert.DeserializeObject<WorkQueueItem>(json);
            File.Delete(firstItem);
            return Task.FromResult<WorkQueueItem?>(item);
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