using System;

using DryIoc;

using LVK.Core.Services;
using LVK.DryIoc;
using LVK.Net.Http.Server;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ConsoleSandbox
{
    internal class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.Bootstrap<LVK.AppCore.Console.ServicesBootstrapper>();
            container.Bootstrap<LVK.Data.Protection.ServicesBootstrapper>();
            container.Bootstrap<LVK.Notifications.Email.ServicesBootstrapper>();
            container.Bootstrap<LVK.Notifications.Pushbullet.ServicesBootstrapper>();
            container.Bootstrap<LVK.Net.Http.Server.ServicesBootstrapper>();
            container.Bootstrap<LVK.Performance.Counters.ServicesBootstrapper>();
            container.Bootstrap<LVK.Configuration.Preferences.ServicesBootstrapper>();

            container.Register<IBackgroundService, WebServerBackgroundService>();
            container.Register<IAuthorizationFilter, MyAuthorizationFilter>();
        }
    }

    internal class MyAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Result = new OkResult();
        }
    }
}