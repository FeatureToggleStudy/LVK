using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

using JetBrains.Annotations;

using LVK.Logging;
using LVK.Reflection;

using Newtonsoft.Json;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Persistence
{
    internal class PersistentData<T> : IPersistentData<T>
        where T: class, new()
    {
        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly Lazy<string> _PersistentFilePath;

        [NotNull]
        private readonly ITypeHelper _TypeHelper;

        [NotNull]
        private readonly object _Lock = new object();

        private int _UpdateScopeLevel;

        [CanBeNull]
        private T _Value;

        public PersistentData([NotNull] ILogger logger, [NotNull] ITypeHelper typeHelper)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _TypeHelper = typeHelper ?? throw new ArgumentNullException(nameof(typeHelper));
            _PersistentFilePath = new Lazy<string>(GetPersistentFilePath, LazyThreadSafetyMode.PublicationOnly);
        }

        public T Value
        {
            get
            {
                if (_Value == null)
                {
                    lock (_Lock)
                    {
                        if (_Value == null)
                        {
                            _Value = LoadFromStorage() ?? new T();
                            HookEvents();
                        }
                    }

                    assume(_Value != null);
                }

                return _Value;
            }
            set
            {
                // ReSharper disable once ConstantNullCoalescingCondition
                _Value = value ?? new T();
                SaveToStorage();
            }
        }

        private void HookEvents()
        {
            if (_Value is INotifyPropertyChanged npc)
                npc.PropertyChanged += ValuePropertyChanged;
        }

        private void ValuePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_UpdateScopeLevel == 0)
                SaveToStorage();
        }

        public void Reload()
        {
            _Value = null;
        }

        public void Save()
        {
            SaveToStorage();
        }

        public void BeginUpdates()
        {
            Interlocked.Increment(ref _UpdateScopeLevel);
        }

        public void EndUpdates()
        {
            if (Interlocked.Decrement(ref _UpdateScopeLevel) == 0)
                SaveToStorage();
        }

        private void SaveToStorage()
        {
            string persistentFilePath = _PersistentFilePath.Value;
            using (_Logger.LogScope(
                LogLevel.Debug, $"Saving persistent value for '{_TypeHelper.NameOf<T>()}' to '{persistentFilePath}'"))
            {
                var json = JsonConvert.SerializeObject(_Value);

                Directory.CreateDirectory(Path.GetDirectoryName(persistentFilePath).NotNull());
                File.WriteAllText(persistentFilePath, json, Encoding.UTF8);
            }
        }

        [CanBeNull]
        private T LoadFromStorage()
        {
            string persistentFilename = _PersistentFilePath.Value;
            using (_Logger.LogScope(
                LogLevel.Debug,
                $"Loading persistent value for '{_TypeHelper.NameOf<T>()}' from '{persistentFilename}'"))
            {
                if (!File.Exists(persistentFilename))
                {
                    _Logger.LogDebug($"Persistent storage for '{_TypeHelper.NameOf<T>()}' not found");
                    return null;
                }

                var json = File.ReadAllText(persistentFilename, Encoding.UTF8);

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        [NotNull]
        private string GetPersistentFilePath()
        {
            string localApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string applicationName = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location);

            var invalidChars = new HashSet<char>(Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()));
            string typeName = new string(
                _TypeHelper.NameOf<T>().ToCharArray().Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());

            return Path.Combine(localApplicationData, applicationName, typeName + ".json");
        }
    }
}