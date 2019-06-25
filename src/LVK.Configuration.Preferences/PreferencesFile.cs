using System;
using System.IO;
using System.Text;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Logging;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Preferences
{
    internal class PreferencesFile : IPreferencesFile
    {
        [NotNull]
        private readonly ILogger _Logger;

        [CanBeNull]
        private readonly string _PreferencesFilePath;

        public PreferencesFile([NotNull] IApplicationDataFolder applicationDataFolder, [NotNull] ILogger logger)
        {
            if (applicationDataFolder == null)
                throw new ArgumentNullException(nameof(applicationDataFolder));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            try
            {
                _PreferencesFilePath = applicationDataFolder.GetDataFilePath("preferences.json");
            }
            catch (InvalidOperationException)
            {
                _PreferencesFilePath = null;
            }
        }

        public JObject Load()
        {
            if (_PreferencesFilePath == null)
                return new JObject();
            
            using (_Logger.LogScope(LogLevel.Debug, "Loading preferences file"))
            {
                try
                {
                    string json = File.ReadAllText(_PreferencesFilePath, Encoding.UTF8);
                    return JObject.Parse(json);
                }
                catch (FileNotFoundException ex)
                {
                    _Logger.LogException(LogLevel.Debug, ex);
                    return new JObject();
                }
                catch (DirectoryNotFoundException ex)
                {
                    _Logger.LogException(LogLevel.Debug, ex);
                    return new JObject();
                }
            }
        }

        public void Save([NotNull] JObject preferences)
        {
            if (preferences == null)
                throw new ArgumentNullException(nameof(preferences));

            if (_PreferencesFilePath == null)
                return;

            string json = preferences.ToString();
            File.WriteAllText(_PreferencesFilePath, json, Encoding.UTF8);
        }
    }
}