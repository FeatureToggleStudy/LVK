using System;
using System.Globalization;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore.Commands;
using LVK.Commands;
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
        private readonly ITestService _TestService;

        [NotNull]
        private readonly ICommandDispatcher _CommandDispatcher;

        [NotNull]
        private readonly IConfigurationElement<string> _Configuration;

        public TestController([NotNull] IConfiguration configuration, [NotNull] ITestService testService, [NotNull] ICommandDispatcher commandDispatcher)
        {
            _TestService = testService ?? throw new ArgumentNullException(nameof(testService));
            _CommandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _Configuration = configuration.Element<string>("Test");
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _CommandDispatcher.Dispatch(new StopApplicationCommand());
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