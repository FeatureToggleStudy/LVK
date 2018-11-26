namespace LVK.Mvvm.Properties
{
    public interface IReadableProperty<out T> : IProperty
    {
        T Value { get; }

        T PeekValue { get; }
    }
}