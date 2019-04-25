using System;

using JetBrains.Annotations;

namespace LVK.Core
{
    internal struct DependentDataKey
    {
        public DependentDataKey([NotNull] Type type, [NotNull] string name)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [NotNull]
        public Type Type { get; }

        [NotNull]
        public string Name { get; }

        public bool Equals(DependentDataKey other) => Type == other.Type && string.Equals(Name, other.Name);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DependentDataKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Type.GetHashCode() * 397) ^ Name.GetHashCode();
            }
        }

        public override string ToString() => $"{Type}, {Name}";
    }
}