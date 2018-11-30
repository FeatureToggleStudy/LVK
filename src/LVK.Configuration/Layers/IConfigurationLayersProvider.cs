using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Configuration.Layers
{
    internal interface IConfigurationLayersProvider
    {
        [NotNull, ItemNotNull]
        IEnumerable<IConfigurationLayer> Provide();
    }
}