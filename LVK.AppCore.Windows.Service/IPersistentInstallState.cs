using System.Collections;

using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Service
{
    internal interface IPersistentInstallState
    {
        void Save([NotNull] IDictionary state);

        [NotNull]
        IDictionary Load();

        void Delete();
    }
}