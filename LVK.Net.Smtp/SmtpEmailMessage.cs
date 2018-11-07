using System;
using System.Collections.Generic;
using System.Linq;

namespace LVK.Net.Smtp
{
    internal class SmtpEmailMessage : ISmtpEmailMessage
    {
        public SmtpEmailMessage(string from, IEnumerable<string> to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(from))
                throw new InvalidOperationException("Missing 'From' name/address for smtp mail message");

            if (to == null)
                throw new InvalidOperationException("Missing 'To' specification for smtp mail message");
            To = to.ToList();
            if (!To.Any())
                throw new InvalidOperationException("Missing 'To' specification for smtp mail message");

            if (string.IsNullOrWhiteSpace(subject))
                throw new InvalidOperationException("Missing 'Subject' for smtp mail message");

            if (string.IsNullOrWhiteSpace(body))
                throw new InvalidOperationException("Missing 'Body' for smtp mail message");

            From = from;
            Subject = subject;
            Body = body;
        }

        public string From { get; }

        public IEnumerable<string> To { get; }

        public string Subject { get; }

        public string Body { get; }
    }
}