using System;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Persistence
{
    [PublicAPI]
    public static class PersistentDataExtensions
    {
        public static IDisposable UpdateScope<T>([NotNull] this IPersistentData<T> persistentData)
            where T : class, new()
        {
            if (persistentData == null)
                throw new ArgumentNullException(nameof(persistentData));

            return new ActionDisposable(persistentData.BeginUpdates, persistentData.EndUpdates);
        }
    }
}