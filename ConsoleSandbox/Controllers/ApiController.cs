using System;

using JetBrains.Annotations;

using LVK.Configuration.Preferences;

using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    public class ApiController : Controller
    {
        [NotNull]
        private readonly IPreferencesManager _Preferences;

        public ApiController([NotNull] IPreferencesManager preferences)
        {
            _Preferences = preferences ?? throw new ArgumentNullException(nameof(preferences));
        }

        [HttpGet]
        [Route("api/customer")]
        public IActionResult GetCustomerName()
        {
            var pref = _Preferences.GetPreference<string>("CustomerName");
            return Ok(pref.Value);
        }

        [HttpPost]
        [Route("api/customer/{name}")]
        public IActionResult SetCustomerName(string name)
        {
            var pref = _Preferences.GetPreference<string>("CustomerName");
            if (name == $"-")
                pref.Reset();
            else
            {
                pref.Value = name;
                pref.Save();
            }

            return Ok(pref.Value);
        }
    }
}