using System.Collections.Generic;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;

namespace LVK.Mvvm.Scopes
{
    [PublicAPI]
    public interface IPropertyReadScope
    {
        void RegisterRead([NotNull] IProperty property);

        [NotNull, ItemNotNull]
        IEnumerable<IProperty> GetReadProperties();
    }
}