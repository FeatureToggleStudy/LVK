using JetBrains.Annotations;

using LVK.Configuration;

namespace LVK.Net.Smtp
{
    internal interface ISmtpClientConfigurationProvider
    {
        [NotNull]
        IConfigurationElement<SmtpClientConfiguration> GetConfiguration([NotNull] string configurationProfileName);
    }
}