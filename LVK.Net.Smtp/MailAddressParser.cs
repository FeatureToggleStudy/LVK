using System.Text.RegularExpressions;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.Net.Smtp
{
    [PublicAPI]
    public static class MailAddressParser
    {
        [NotNull]
        private static readonly Regex _AddressPattern = new Regex(@"^\s*(?<display>[^<]+)\s*<(?<address>[^>]+)>\s*$");
        
        public static (bool success, string displayName, string mailAddress) TryParse([NotNull] string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return (false, default, default);

            var ma = _AddressPattern.Match(address);
            if (ma.Success)
            {
                JetBrainsHelpers.assume(ma.Groups["display"] != null && ma.Groups["address"] != null);
                return (true, ma.Groups["display"].Value, ma.Groups["address"].Value);
            }

            return (true, address, address);

        }
    }
}