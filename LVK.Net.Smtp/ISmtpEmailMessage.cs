using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    [PublicAPI]
    public interface ISmtpEmailMessage
    {
        [NotNull]
        string From { get; }

        [NotNull, ItemNotNull]
        IEnumerable<string> To { get; }

        [NotNull]
        string Subject { get; }

        [NotNull]
        string Body { get; }
    }
}