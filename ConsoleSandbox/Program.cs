using System.Threading.Tasks;

using LVK.AppCore.Web;

namespace ConsoleSandbox
{
    static class Program
    {
        public static Task Main(string[] args)
        {
            return WebAppBootstrapper.RunWebApiAsync<ServicesBootstrapper>(args);
        }
    }
}