using System.Threading.Tasks;

using LVK.AppCore.Web;

namespace ConsoleSandbox
{
    static class Program
    {
        static Task Main() => WebAppBootstrapper.RunWebApiAsync<ServicesBootstrapper>(new string[0]);
    }
}