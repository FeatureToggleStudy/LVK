using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class StackExtensions
    {
        [CanBeNull]
        public static T PeekOrDefault<T>([NotNull] this Stack<T> stack, T defaultValue = default)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            if (stack.Count > 0)
                return stack.Peek();

            return defaultValue;
        }

        [CanBeNull]
        public static T PeekOrDefault<T>([NotNull] this Stack<T> stack, [NotNull] Func<T> getDefaultValue)
        {
            if (stack == null)
                throw new ArgumentNullException(nameof(stack));

            if (getDefaultValue == null)
                throw new ArgumentNullException(nameof(getDefaultValue));

            if (stack.Count > 0)
                return stack.Peek();

            return getDefaultValue();
        }
    }
}