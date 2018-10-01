using JetBrains.Annotations;

using LVK.Configuration;

using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [NotNull]
        private readonly IConfigurationElement<string> _Configuration;

        public TestController([NotNull] IConfiguration configuration)
        {
            _Configuration = configuration.Element<string>("Test");
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_Configuration.Value());
        }
    }
}