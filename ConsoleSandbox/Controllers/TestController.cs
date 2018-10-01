using System;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.Core.Services;

using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly IConfigurationElement<string> _Configuration;

        public TestController([NotNull] IConfiguration configuration, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _Configuration = configuration.Element<string>("Test");
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ApplicationLifetimeManager.SignalGracefulTermination();
            return Ok(_Configuration.Value());
        }
    }
}