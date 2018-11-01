using System;
using System.IO;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using LVK.Configuration.Helpers;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Layers.JsonFile
{
    internal class JsonFileConfigurationLayer : IConfigurationLayer, IJsonFileConfigurationLayer
    {
        // ReSharper disable InconsistentNaming
        private const int ERROR_SHARING_VIOLATION = -2147024864;
        // ReSharper restore InconsistentNaming

        [NotNull]
        private readonly Encoding _Encoding;

        private DateTime _PreviousLastWriteTime = DateTime.MinValue;
        private JObject _Configuration;

        [NotNull]
        private Action _StateCheck;

        [NotNull]
        private readonly string _FilePath;

        public JsonFileConfigurationLayer(
            [NotNull] string filename, [NotNull] Encoding encoding, bool isOptional)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            _FilePath = ConfigurationFilePath.GetFullPath(filename);
            _Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            IsOptional = isOptional;

            _StateCheck = StateCheckForFileComingIntoExistence;
        }

        private void StateCheckForFileComingIntoExistence()
        {
            if (File.Exists(_FilePath))
            {
                var configuration = LoadConfigurationFromFile();
                if (configuration != null)
                {
                    _Configuration = configuration;
                    _PreviousLastWriteTime = File.GetLastWriteTimeUtc(_FilePath);
                    _StateCheck = StateCheckForFileModification;
                }
            }
            else
                ThrowIfNotOptional();
        }

        private void StateCheckForFileModification()
        {
            if (!File.Exists(_FilePath))
            {
                _StateCheck = StateCheckForFileComingIntoExistence;
                _Configuration = null;
                ThrowIfNotOptional();
            }
            else
            {
                var lastWriteTime = File.GetLastWriteTimeUtc(_FilePath); 
                if (lastWriteTime != _PreviousLastWriteTime)
                {
                    _PreviousLastWriteTime = lastWriteTime;
                    _Configuration = LoadConfigurationFromFile();
                    if (_Configuration == null)
                    {
                        _StateCheck = StateCheckForFileComingIntoExistence;
                        ThrowIfNotOptional();
                    }
                }
            }
        }

        private void ThrowIfNotOptional()
        {
            if (!IsOptional)
                throw new InvalidOperationException($"configuration file '{_FilePath}' does not exist");
        }

        private JObject LoadConfigurationFromFile()
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return JObject.Parse(File.ReadAllText(_FilePath, _Encoding));
                }
                catch (JsonReaderException)
                {
                    break;
                }
                catch (FileNotFoundException)
                {
                    break;
                }
                catch (IOException ex) when (ex.HResult == ERROR_SHARING_VIOLATION)
                {
                    retryCount++;
                    if (retryCount < 10)
                        Thread.Sleep(10);
                    else
                        break;
                }
                catch
                {
                    break;
                }
            }

            return null;
        }

        public JObject Configuration
        {
            get
            {
                _StateCheck();
                return _Configuration;
            }
        }

        public string GetJsonFilePath() => _FilePath;

        public bool IsOptional { get; }
    }
}