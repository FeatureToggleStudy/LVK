namespace ConsoleSandbox
{
    public interface ICounterHolder
    {
        int CurrentValue { get; set; }
    }

    internal class CounterHolder : ICounterHolder
    {
        public int CurrentValue { get; set; }
    }
}