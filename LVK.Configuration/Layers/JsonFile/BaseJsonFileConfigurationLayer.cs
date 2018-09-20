using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json.Linq;

using NodaTime;

namespace LVK.Configuration.Layers.JsonFile
{
    internal abstract class BaseJsonFileConfigurationLayer : IConfigurationLayer
    {
        [NotNull]
        private readonly IClock _Clock;

        [NotNull]
        private readonly Encoding _Encoding;

        private bool _FileExistedLastTime;
        private Instant _PreviousLastWriteTime = Instant.MinValue;
        private JObject _Configuration = new JObject();

        protected BaseJsonFileConfigurationLayer(
            [NotNull] IClock clock, [NotNull] string filename, [NotNull] Encoding encoding)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            _Clock = clock ?? throw new ArgumentNullException(nameof(clock));
            Filename = ConfigurationFilePath.GetFullPath(filename);
            _Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        [NotNull]
        protected string Filename { get; }

        private void ReloadIfNecessary()
        {
            bool fileExistsNow = File.Exists(Filename);
            if (_FileExistedLastTime && fileExistsNow)
                ReloadConfigurationFromFileIfChangedSinceLastTime();
            else if (_FileExistedLastTime && !fileExistsNow)
                ClearConfigurationBecauseFileHasBeenDeleted();
            else if (!_FileExistedLastTime && fileExistsNow)
                LoadConfigurationFromFile();
            
            if (_Configuration == null)
                FileDoesNotExist();

            _FileExistedLastTime = fileExistsNow;
        }

        private void ReloadConfigurationFromFileIfChangedSinceLastTime()
        {
            var lastWriteTime = GetFileLastWriteTime();
            if (lastWriteTime > _PreviousLastWriteTime)
                LoadConfigurationFromFile();
        }

        private void ClearConfigurationBecauseFileHasBeenDeleted()
        {
            _PreviousLastWriteTime = _Clock.GetCurrentInstant();
            _Configuration = null;
        }

        private void LoadConfigurationFromFile()
        {
            _Configuration = JObject.Parse(File.ReadAllText(Filename, _Encoding));
            _PreviousLastWriteTime = Instant.Max(GetFileLastWriteTime(), _PreviousLastWriteTime);
        }

        private Instant GetFileLastWriteTime() => Instant.FromDateTimeUtc(File.GetLastWriteTimeUtc(Filename));

        public JObject Configuration
        {
            get
            {
                ReloadIfNecessary();
                return _Configuration;
            }
        }
        
        protected abstract void FileDoesNotExist();
    }
}