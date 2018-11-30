using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.Core;
using LVK.Mvvm.Properties;

namespace LVK.Mvvm.Scopes
{
    internal class PropertyReadScope : IPropertyReadScope
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<IProperty> _Properties = new HashSet<IProperty>(new ObjectReferenceEqualityComparer());
        
        public void RegisterRead(IProperty property)
        {
            _Properties.Add(property);
        }

        public IEnumerable<IProperty> GetReadProperties() => _Properties.ToList();
    }
}