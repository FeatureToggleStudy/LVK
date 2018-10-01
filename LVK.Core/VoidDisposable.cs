using System;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public class VoidDisposable : IDisposable
    {
        public void Dispose()
        {
        }
    }
}