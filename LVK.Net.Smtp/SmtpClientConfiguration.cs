// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LVK.Net.Smtp
{
    internal class SmtpClientConfiguration
    {
        public SmtpClientServerConfiguration Server { get; set; }

        public SmtpClientAuthenticationConfiguration Authentication { get; set; }

        public bool IsValid() => Server?.IsValid() == true && Authentication?.IsValid() == true;
    }
}