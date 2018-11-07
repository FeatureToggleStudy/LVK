// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LVK.Net.Smtp
{
    internal class SmtpClientAuthenticationConfiguration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsValid() => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
    }
}