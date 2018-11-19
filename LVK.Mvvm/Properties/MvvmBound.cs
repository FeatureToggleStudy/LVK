using System;

using JetBrains.Annotations;

namespace LVK.Mvvm.Properties
{
    internal abstract class MvvmBound
    {
        protected MvvmBound([NotNull] IMvvmContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [NotNull]
        protected IMvvmContext Context { get; }
    }
}