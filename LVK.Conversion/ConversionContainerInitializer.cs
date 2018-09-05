using System;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Conversion
{
    internal class ConversionContainerInitializer : IContainerInitializer
    {
        [NotNull]
        private readonly IValueConverter _ValuerConverter;

        public ConversionContainerInitializer([NotNull] IValueConverter valuerConverter)
        {
            _ValuerConverter = valuerConverter ?? throw new ArgumentNullException(nameof(valuerConverter));
        }

        public void Initialize()
        {
            ValueConverter.Instance = _ValuerConverter;
        }
    }
}