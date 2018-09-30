using System;

using JetBrains.Annotations;

namespace LVK.WorkQueues.FileBased
{
    internal class FileWorkQueueConfiguration
    {
        [NotNull]
        private string _Path = String.Empty;

        [NotNull]
        public string Path
        {
            get => _Path;
            // ReSharper disable once ConstantNullCoalescingCondition
            set => _Path = value ?? String.Empty;
        }

        public bool IsEnabled { get; set; }
    }
}