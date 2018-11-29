using System;
using System.Collections.Generic;
using System.Linq;

namespace LVK.Core.Services
{
    internal class ApplicationContext : IApplicationContext
    {
        public ApplicationContext()
        {
            CommandLineArguments = Environment.GetCommandLineArgs().Skip(1).ToArray();
        }

        public IEnumerable<string> CommandLineArguments { get; }
    }
}