using System.Text;

namespace LVK.AppCore.Console
{
    internal static class ConsoleAppConfigurator
    {
        internal static void Configure()
        {
            System.Console.InputEncoding = Encoding.Unicode;
            System.Console.OutputEncoding = Encoding.Unicode;
        }
    }
}