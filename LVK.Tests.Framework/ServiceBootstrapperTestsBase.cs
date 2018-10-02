using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

using NUnit.Framework;

namespace LVK.Tests.Framework
{
    [PublicAPI]
    public abstract class ServiceBootstrapperTestsBase<T>
        where T: class, IServicesBootstrapper
    {
        protected static IEnumerable<Assembly> GetReferencedAssemblies()
        {
            var result = new List<Assembly>();
            foreach (var assemblyName in typeof(T).Assembly.GetReferencedAssemblies())
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    result.Add(assembly);
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }

        protected static IEnumerable<Type> ServiceBootstrappersInReferencedAssemblies()
        {
            return
                from assembly in GetReferencedAssemblies()
                from type in assembly.GetTypes()
                where type.Name == "ServicesBootstrapper"
                select type;
        }

        protected void VerifyDependency(Type servicesBootstrapperType)
        {
            var container = ContainerFactory.Create().Bootstrap<T>();

            var register = container.Resolve<IServicesBootstrapperRegister>();
            bool isRegistered = register.IsRegistered(servicesBootstrapperType);

            Assert.That(
                isRegistered, Is.True, $"Dependency on assembly {servicesBootstrapperType.Assembly.GetName().Name} but services bootstrapper not called");
        }
    }
}