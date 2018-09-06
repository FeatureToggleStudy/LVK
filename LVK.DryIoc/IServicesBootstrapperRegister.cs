namespace LVK.DryIoc
{
    internal interface IServicesBootstrapperRegister
    {
        bool TryAdd<T>();
    }
}