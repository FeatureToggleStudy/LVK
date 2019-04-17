using JetBrains.Annotations;

using SQLitePCL;

namespace LVK.Data.Sqlite
{
    internal class SQLitePclInitializer : ISQLitePCLInitializer
    {
        [NotNull]
        private readonly object _Lock = new object();
        
        private volatile bool _IsInitialized;
        
        public void InitializeOnce()
        {
            if (_IsInitialized)
                return;
            
            lock (_Lock)
            {
                Batteries.Init();
                _IsInitialized = true;
            }
        }
    }
}