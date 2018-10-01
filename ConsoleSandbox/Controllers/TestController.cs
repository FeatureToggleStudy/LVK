using System;
using System.Globalization;

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
        private readonly ITestService _TestService;

        [NotNull]
        private readonly IConfigurationElement<string> _Configuration;

        public TestController([NotNull] IConfiguration configuration, [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ITestService testService)
        {
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
            _TestService = testService ?? throw new ArgumentNullException(nameof(testService));
            _Configuration = configuration.Element<string>("Test");
        }

        [HttpGet]
        public IActionResult Get()
        {
            _ApplicationLifetimeManager.SignalGracefulTermination();
            return Ok(_Configuration.Value() + ": " + _TestService.Now());
        }
    }

    public interface ITestService
    {
        string Now();
    }

    internal class TestService : ITestService, IDisposable
    {
        public string Now() => DateTime.Now.ToString(CultureInfo.InvariantCulture);

        public void Dispose()
        {
            Console.WriteLine("Disposed");
        }
    }
}