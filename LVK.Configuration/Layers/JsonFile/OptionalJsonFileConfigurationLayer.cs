using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class OptionalJsonFileConfigurationLayer : IConfigurationLayer
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly string _Filename;

        [NotNull]
        private readonly Encoding _Encoding;

        private bool _FileExistedLastTime;
        private Instant _LastChangedAt = Instant.MinValue;
        private JObject _Configuration = new JObject();

        public OptionalJsonFileConfigurationLayer(
            [NotNull] IClock clock, [NotNull] string filename, [NotNull] Encoding encoding)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            _Filename = ConfigurationFilePath.GetFullPath(filename);
            _Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public Instant LastChangedAt
        {
            get
            {
                ReloadIfNecessary();
                return _LastChangedAt;
            }
        }

        private void ReloadIfNecessary()
        {
            bool fileExistsNow = File.Exists(_Filename);
            if (_FileExistedLastTime && fileExistsNow)
                ReloadConfigurationFromFileIfChangedSinceLastTime();
            else if (_FileExistedLastTime && !fileExistsNow)
                ClearConfigurationBecauseFileHasBeenDeleted();
            else if (!_FileExistedLastTime && fileExistsNow)
                LoadConfigurationFromFile();

            _FileExistedLastTime = fileExistsNow;
        }

        private void ReloadConfigurationFromFileIfChangedSinceLastTime()
        {
            var lastWriteTime = GetFileLastWriteTime();
            if (lastWriteTime > _LastChangedAt)
            {
                Console.WriteLine($"detected change in '{_Filename}'");
                LoadConfigurationFromFile();
            }
        }

        private void ClearConfigurationBecauseFileHasBeenDeleted()
        {
            Console.WriteLine($"clearing configuration from '{_Filename}', file has been deleted");
            _LastChangedAt = _Clock.GetCurrentInstant();
            _Configuration = new JObject();
        }

        private void LoadConfigurationFromFile()
        {
            Console.WriteLine($"loading configuration from '{_Filename}'");
            _Configuration = JObject.Parse(File.ReadAllText(_Filename, _Encoding));
            _LastChangedAt = Instant.Max(GetFileLastWriteTime(), _LastChangedAt);
        }

        private Instant GetFileLastWriteTime() => Instant.FromDateTimeUtc(File.GetLastWriteTimeUtc(_Filename));

        public JObject GetConfiguration()
        {
            ReloadIfNecessary();
            return _Configuration;
        }
    }
}