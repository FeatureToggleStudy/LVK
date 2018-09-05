using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Reflection
{
    [PublicAPI]
    public class TypeHelper : ITypeHelper
    {
        [NotNull, ItemNotNull]
        private readonly List<ITypeNameRule> _NameOfRules;

        // ReSharper disable once NotNullMemberIsNotInitialized
        static TypeHelper()
        {
            new ContainerBuilder().Register<ServicesRegistrant>().Build();
        }

        public TypeHelper([NotNull, ItemNotNull] IEnumerable<ITypeNameRule> nameOfRules)
        {
            _NameOfRules = nameOfRules.OrderBy(r => r.Priority).ToList();
        }

        string ITypeHelper.TryGetNameOf(Type type, NameOfTypeOptions options)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _NameOfRules.Select(r => r.TryGetNameOfType(type, this, options)).FirstOrDefault(n => !(n is null));
        }

        [NotNull]
        public static ITypeHelper Instance { get; internal set; }
    }
}