using System;

using JetBrains.Annotations;

namespace LVK.DataStructures
{
    [PublicAPI]
    public class CyclicDependenciesException : InvalidOperationException
    {
        public CyclicDependenciesException([NotNull] string message) : base(message)
        {
        }
    }
}