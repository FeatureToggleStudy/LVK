using System;
using System.Collections.Generic;

using DryIoc;

using JetBrains.Annotations;

namespace LVK.DryIoc
{
    [PublicAPI]
    public class ContainerBuilder : IContainerBuilder
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<Type> _RegistrantTypes = new HashSet<Type>();
        
        [NotNull, ItemNotNull]
        private readonly List<IServicesRegistrant> _Registrants = new List<IServicesRegistrant>();
        
        public IContainerBuilder Register<T>()
            where T: class, IServicesRegistrant, new()
        {
            if (_RegistrantTypes.Add(typeof(T)))
            {
                var registrant = new T();
                _Registrants.Add(registrant);
                registrant.Register(this);
            }

            return this;
        }

        public IContainer Build()
        {
            var container = new Container();

            foreach (var registrant in _Registrants)
                registrant.Register(container);

            return container;
        }
    }
}