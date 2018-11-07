using System.Threading.Tasks;

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    [PublicAPI]
    public interface ISmtpClient
    {
        [NotNull]
        ISmtpEmailMessageBuilder CreateEmailMessageBuilder();

        [NotNull]
        Task SendEmailMessageAsync([NotNull] ISmtpEmailMessage emailMessage);
    }
}