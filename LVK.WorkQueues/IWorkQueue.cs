using JetBrains.Annotations;

namespace LVK.WorkQueues
{
    [PublicAPI]
    public interface IWorkQueue
    {
        void Enqueue<T>([NotNull] T item)
            where T: class;
    }
}