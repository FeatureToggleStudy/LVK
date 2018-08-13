using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

        private static int _NextId;
        
        [NotNull]
        private static readonly Dictionary<int, string> _Values = new Dictionary<int, string>();

        public ValuesController([NotNull] ILogger<ValuesController> logger)
         {
             _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
         }

        // GET api/values
        [HttpGet]
        public Task<IEnumerable<string>> Get()
        {
            lock (_Values)
                return Task.FromResult<IEnumerable<string>>(_Values.Values);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            _Logger.Log(LogLevel.Information, $"Get({id})");
            lock (_Values)
            {
                if (!_Values.TryGetValue(id, out string value))
                    return NotFound();

                return Ok(value);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _Logger.Log(LogLevel.Information, $"Post({value})");
            
            int id = Interlocked.Increment(ref _NextId);
            lock (_Values)
                _Values[id] = value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            _Logger.Log(LogLevel.Information, $"Put(id, {value})");
            lock (_Values)
                _Values[id] = value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _Logger.Log(LogLevel.Information, $"Delete({id})");
            lock (_Values)
                _Values.Remove(id);
        }
    }
}