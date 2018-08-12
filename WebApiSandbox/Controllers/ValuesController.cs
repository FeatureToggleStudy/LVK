using System;
using System.Collections.Generic;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiSandbox.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        [NotNull]
        private readonly ILogger<ValuesController> _Logger;

        public ValuesController([NotNull] ILogger<ValuesController> logger)
         {
             _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
         }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _Logger.Log(LogLevel.Information, "Get()");
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            _Logger.Log(LogLevel.Information, $"Get({id})");
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}