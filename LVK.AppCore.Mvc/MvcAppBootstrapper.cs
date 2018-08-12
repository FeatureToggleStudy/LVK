using System;
using System.Runtime.InteropServices;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using JetBrains.Annotations;

using LVK.AppCore.Console;
using LVK.Core;
using LVK.DryIoc;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.AppCore.Mvc
{
    public static class MvcAppBootstrapper
    {
        public static IServiceProvider Bootstrap<T>([NotNull] IServiceCollection services)
            where T: class, IServiceBootstrapper
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            
            return new Container()
                .WithDependencyInjectionAdapter(services).NotNull()
                .Bootstrap<LVK.Core.Services.ServiceBootstrapper>()
                .Bootstrap<T>()
                .ConfigureServiceProvider<DummyCompositionRoot>();
        }
    }
}