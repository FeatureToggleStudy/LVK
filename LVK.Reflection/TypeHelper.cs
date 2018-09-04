using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace LVK.Reflection
{
    internal class TypeHelper : ITypeHelper
    {
        [NotNull, ItemNotNull]
        private readonly List<ITypeNameRule> _NameOfRules;

        public TypeHelper([NotNull, ItemNotNull] IEnumerable<ITypeNameRule> nameOfRules)
        {
            _NameOfRules = nameOfRules.OrderBy(r => r.Priority).ToList();
        }

        public string TryGetNameOf(Type type, NameOfTypeOptions options = NameOfTypeOptions.Default)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _NameOfRules.Select(r => r.TryGetNameOfType(type, this, options)).FirstOrDefault(n => !(n is null));
        }
    }
}