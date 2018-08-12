namespace LVK.DryIoc
{
    internal interface IContainerBootstrapperRegister
    {
        bool TryAddBootstrapper<T>();
    }
}