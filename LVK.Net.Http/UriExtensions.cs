using System;

using JetBrains.Annotations;

namespace LVK.Net.Http
{
    [PublicAPI]
    public static class UriExtensions
    {
        public static Uri Append([NotNull] this Uri baseUri, [NotNull, ItemNotNull] params Uri[] additionalUris)
        {
            if (baseUri is null)
                throw new ArgumentNullException(nameof(baseUri));

            if (additionalUris is null)
                throw new ArgumentNullException(nameof(additionalUris));

            foreach (var additionalUri in additionalUris)
                if (!string.IsNullOrWhiteSpace(additionalUri?.ToString()))
                    baseUri = new Uri(baseUri, additionalUri);

            return baseUri;
        }

        [NotNull]
        public static Uri Append([NotNull] this Uri baseUri, [NotNull, ItemNotNull] params string[] additionalUris)
        {
            if (baseUri is null)
                throw new ArgumentNullException(nameof(baseUri));

            if (additionalUris is null)
                throw new ArgumentNullException(nameof(additionalUris));

            foreach (var additionalUri in additionalUris)
                if (!string.IsNullOrWhiteSpace(additionalUri))
                    baseUri = new Uri(baseUri, additionalUri);

            return baseUri;
        }
    }
}