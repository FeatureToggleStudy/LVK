using DryIoc;

namespace LVK.DryIoc.Tests
{
    public class DummyBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            BootstrapCallCounter++;
        }

        public static void Reset() => BootstrapCallCounter = 0;

        public static int BootstrapCallCounter { get; private set; }
    }
}