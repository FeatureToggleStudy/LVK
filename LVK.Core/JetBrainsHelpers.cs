using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

using JetBrains.Annotations;

namespace LVK.Core
{
    [PublicAPI]
    public static class JetBrainsHelpers
    {
        [ContractAnnotation("instance:null => halt")]
        [NotNull]
        public static T NotNull<T>(this T instance)
            where T: class
            => instance ?? throw new ArgumentNullException(nameof(instance));

        [ContractAnnotation("expression:false => halt")]
        [Conditional("DEBUG")]
        // ReSharper disable once InconsistentNaming
        public static void assume(bool expression)
        {
        }
    }
}