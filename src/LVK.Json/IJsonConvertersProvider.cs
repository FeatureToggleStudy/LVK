using System.Collections.Generic;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Json
{
    [PublicAPI]
    public interface IJsonConvertersProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<JsonConverter> GetJsonConverters();
    }
}