using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.Configuration.Preferences
{
    internal interface IPreferencesFile
    {
        [NotNull]
        JObject Load();

        void Save(JObject preferences);
    }
}