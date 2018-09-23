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
    internal class JsonFileConfigurationLayer : IConfigurationLayer
    {
        // ReSharper disable InconsistentNaming
        private const int ERROR_SHARING_VIOLATION = -2147024864;
        // ReSharper restore InconsistentNaming

        [NotNull]
        private readonly Encoding _Encoding;

        private readonly bool _IsOptional;

        private DateTime _PreviousLastWriteTime = DateTime.MinValue;
        private JObject _Configuration = new JObject();

        [NotNull]
        private Action _StateCheck;

        public JsonFileConfigurationLayer(
            [NotNull] string filename, [NotNull] Encoding encoding, bool isOptional)
        {
            if (filename == null)
                throw new ArgumentNullException(nameof(filename));

            Filename = ConfigurationFilePath.GetFullPath(filename);
            _Encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
            _IsOptional = isOptional;

            _StateCheck = StateCheckForFileComingIntoExistence;
        }

        [NotNull]
        protected string Filename { get; }

        private void StateCheckForFileComingIntoExistence()
        {
            if (File.Exists(Filename))
            {
                _StateCheck = StateCheckForFileModification;
                LoadConfigurationFromFile();
            }
        }

        private void StateCheckForFileModification()
        {
            if (!File.Exists(Filename))
            {
                _StateCheck = StateCheckForFileComingIntoExistence;
                _Configuration = null;
                ThrowIfNotOptional();
            }
            else
            {
                var lastWriteTime = File.GetLastWriteTimeUtc(Filename); 
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
            if (!_IsOptional)
                throw new InvalidOperationException($"configuration file '{Filename}' does not exist");
        }

        private JObject LoadConfigurationFromFile()
        {
            int retryCount = 0;
            while (true)
            {
                try
                {
                    return JObject.Parse(File.ReadAllText(Filename, _Encoding));
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
    }
}