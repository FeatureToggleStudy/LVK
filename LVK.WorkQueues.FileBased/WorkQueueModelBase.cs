using System;

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

namespace LVK.WorkQueues.FileBased
{
    internal abstract class WorkQueueModelBase
    {
        [NotNull]
        private JObject _Payload = new JObject();

        [NotNull]
        private string _Type = String.Empty;

        [UsedImplicitly]
        public JObject Payload
        {
            get => _Payload;
            // ReSharper disable once ConstantNullCoalescingCondition
            set => _Payload = value ?? new JObject();
        }

        [UsedImplicitly]
        public string Type
        {
            get => _Type;
            // ReSharper disable once ConstantNullCoalescingCondition
            set => _Type = value ?? String.Empty;
        }
    }
}