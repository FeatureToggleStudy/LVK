using System;
using System.Threading.Tasks;

using LVK.AppCore.Tray;

namespace ConsoleSandbox
{
    static class Program
    {
        static async Task<int> Main()
        {
            try
            {
                return await TrayAppBootstrapper.RunAsync<ServicesBootstrapper>();
            }
            finally
            {
                Console.WriteLine("Here");
            }
        }
    }
}