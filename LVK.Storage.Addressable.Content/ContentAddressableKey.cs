using System;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using Newtonsoft.Json;

namespace LVK.Storage.Addressable.Content
{
    [PublicAPI]
    public struct ContentAddressableKey
    {
        [NotNull]
        private static readonly Regex _HashPattern = new Regex(@"^[a-f0-9]{40}$", RegexOptions.IgnoreCase);

        public const string HashPropertyName = "cas::key";

        [CanBeNull]
        private readonly string _Hash;

        [JsonConstructor]
        public ContentAddressableKey(
            [CanBeNull, JsonProperty(HashPropertyName)]
            string hash)
        {
            if (hash == null)
            {
                _Hash = null;
                return;
            }

            if (!IsHashValid(hash))
                throw new ArgumentException("hash must be 40 characters and consist of 0-9 or a-f symbols", nameof(hash));

            _Hash = hash.ToLowerInvariant();
        }

        public bool ShouldSerializeHash() => IsValid;
        
        [NotNull]
        [JsonProperty(HashPropertyName)]
        public string Hash => _Hash ?? throw new InvalidOperationException("This is not a valid key, reading Hash is not possible");

        [JsonIgnore]
        public bool IsValid => _Hash != null;

        public static ContentAddressableKey TryParse([NotNull] string hash)
        {
            if (!IsHashValid(hash))
                return default;

            return new ContentAddressableKey(hash);
        }

        private static bool IsHashValid(string hash) => _HashPattern.IsMatch(hash ?? string.Empty);

        public static readonly ContentAddressableKey InvalidKey;

        public bool Equals(ContentAddressableKey other) => string.Equals(_Hash, other._Hash);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is ContentAddressableKey other && Equals(other);
        }

        public override int GetHashCode() => (_Hash != null ? _Hash.GetHashCode() : 0);

        public override string ToString() => _Hash ?? "<invalid key>";
    }
}