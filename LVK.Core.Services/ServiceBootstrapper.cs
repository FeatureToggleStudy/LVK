using DryIoc;

using LVK.DryIoc;

using NodaTime;

namespace LVK.Core.Services
{
    public class ServiceBootstrapper : IServiceBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            container.UseInstance<IClock>(SystemClock.Instance);
        }
    }
}