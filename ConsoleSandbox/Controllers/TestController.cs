using System;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    public class TestModel
    {
        public TestModel(int counter) => Counter = counter;

        public int Counter { get; }
    }

    public class TestController : Controller
    {
        [NotNull]
        private readonly ICounterHolder _CounterHolder;

        public TestController([NotNull] ICounterHolder counterHolder)
        {
            _CounterHolder = counterHolder ?? throw new ArgumentNullException(nameof(counterHolder));
        }

        // GET
        [Route("/test")]
        public IActionResult Index()
        {
            return View(new TestModel(_CounterHolder.CurrentValue));
        }
    }
}