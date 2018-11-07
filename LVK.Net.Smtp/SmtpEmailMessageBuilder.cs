using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Net.Smtp
{
    internal class SmtpEmailMessageBuilder : ISmtpEmailMessageBuilder
    {
        [NotNull]
        private string _From = "";

        [NotNull]
        private string _Subject = "";

        [NotNull]
        private string _Body = "";

        public string From
        {
            get => _From;

            // ReSharper disable once ConstantNullCoalescingCondition
            set => _From = value ?? "";
        }

        public List<string> To { get; } = new List<string>();

        public string Subject
        {
            get => _Subject;

            // ReSharper disable once ConstantNullCoalescingCondition
            set => _Subject = value ?? "";
        }

        public string Body
        {
            get => _Body;

            // ReSharper disable once ConstantNullCoalescingCondition
            set => _Body = value ?? "";
        }

        public ISmtpEmailMessage Build() => new SmtpEmailMessage(From, To, Subject, Body);
    }
}