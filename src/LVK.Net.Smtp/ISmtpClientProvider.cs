using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    [PublicAPI]
    public interface ISmtpClientProvider
    {
        [NotNull]
        ISmtpClient Provide([NotNull] string configurationProfileName);
    }
}