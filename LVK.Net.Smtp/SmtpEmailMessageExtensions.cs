using System;
using System.Net.Mail;
using System.Text;

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    internal static class SmtpEmailMessageExtensions
    {
        [NotNull]
        public static MailMessage CreateFrameworkMailMessage([NotNull] this ISmtpEmailMessage source)
        {
            var target = new MailMessage { BodyEncoding = Encoding.UTF8, SubjectEncoding = Encoding.UTF8 };

            var (success, displayName, mailAddress) = MailAddressParser.TryParse(source.From);
            if (!success)
                throw new InvalidOperationException("Unable to assign 'From' address for smtp message, missing or invalid value");
            
            target.From = new MailAddress(displayName, mailAddress, Encoding.UTF8);

            foreach (var to in source.To)
            {
                (success, displayName, mailAddress) = MailAddressParser.TryParse(to);
                if (!success)
                    throw new InvalidOperationException("Unable to assign 'To' addresses for smtp message, missing or invalid value");

                target.To.Add(new MailAddress(displayName, mailAddress, Encoding.UTF8));
            }

            target.Subject = source.Subject;
            target.Body = source.Body;

            return target;
        }
    }
}