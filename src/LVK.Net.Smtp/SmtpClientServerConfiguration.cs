// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LVK.Net.Smtp
{
    internal class SmtpClientServerConfiguration
    {
        public string Address { get; set; }

        public int Port { get; set; } = 587;

        public bool IsValid() => !string.IsNullOrWhiteSpace(Address) && Port != 0;
    }
}