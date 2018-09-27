using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DryIoc;
using DryIoc.Microsoft.DependencyInjection;

using LVK.AppCore.Web;
using LVK.DryIoc;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleSandbox
{
    static class Program
    {
        public static Task Main(string[] args)
        {
            return WebAppBootstrapper.RunWebApiAsync<ServicesBootstrapper>(typeof(Program), args);
        }
    }
}