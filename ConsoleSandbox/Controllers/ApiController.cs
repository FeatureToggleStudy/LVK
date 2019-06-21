using System;
using System.Net;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    public class ApiController : Controller
    {
        [ClaimRequirement]
        [Route("api/test")]
        public IActionResult Index()
        {
            Console.WriteLine("Nhập Chuỗi Ký Tự :");
            return Ok(new { a = 42, b = "Meaning of life" });
        }
    }
}