using System;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.AppCore.Web
{
    internal class WrapServiceProvider : IServiceProvider
    {
        [NotNull]
        private readonly IServiceProvider _ServiceProvider;

        public WrapServiceProvider([NotNull] IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public object GetService(Type serviceType)
        {
            Console.WriteLine($"resolve: {serviceType.FullName}");
            return _ServiceProvider.GetService(serviceType);
        }
    }
}