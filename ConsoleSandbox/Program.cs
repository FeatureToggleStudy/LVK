﻿using System.Threading.Tasks;

using LVK.AppCore.Console;

namespace ConsoleSandbox
{
    static class Program
    {
        static Task<int> Main(string[] args) => ConsoleAppBootstrapper.Execute<CompositionRoot>();
    }
}