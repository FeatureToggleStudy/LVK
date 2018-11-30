using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    [PublicAPI]
    public interface ISmtpEmailMessageBuilder
    {
        [NotNull]
        string From { get; set; }

        [NotNull, ItemNotNull]
        List<string> To { get; }

        [NotNull]
        string Subject { get; set; }

        [NotNull]
        string Body { get; set; }

        [NotNull]
        ISmtpEmailMessage Build();
    }
}