// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    internal class SmtpClientServerConfiguration
    {
        [UsedImplicitly]
        public string Address { get; set; }

        [UsedImplicitly]
        public int Port { get; set; } = 587;

        public bool IsValid() => !string.IsNullOrWhiteSpace(Address) && Port != 0;
    }
}