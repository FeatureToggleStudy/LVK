using System;
using System.Collections.Generic;
using System.Linq;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.Reflection
{
    [PublicAPI]
    public class TypeHelper : ITypeHelper
    {
        [NotNull, ItemNotNull]
        private readonly List<ITypeNameRule> _NameOfRules;

        [NotNull]
        private static readonly object _InstanceLock = new object();

        [CanBeNull]
        private static ITypeHelper _Instance;

        public TypeHelper([NotNull, ItemNotNull] IEnumerable<ITypeNameRule> nameOfRules)
        {
            _NameOfRules = nameOfRules.OrderBy(r => r.Priority).ToList();
            Instance = this;
        }

        string ITypeHelper.TryGetNameOf(Type type, NameOfTypeOptions options)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _NameOfRules.Select(r => r.TryGetNameOfType(type, this, options)).FirstOrDefault(n => !(n is null));
        }

        [NotNull]
        public static ITypeHelper Instance
        {
            get
            {
                if (_Instance is null)
                {
                    lock (_InstanceLock)
                    {
                        if (_Instance is null)
                        {
                            // ReSharper disable once HeuristicUnreachableCode
                            new Container().Bootstrap<ServicesBootstrapper>().Resolve<ITypeHelper>();
                        }
                    }

                    assume(_Instance != null);
                }

                return _Instance;
            }
            internal set => _Instance = value;
        }
    }
}