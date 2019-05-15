using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.Configuration;
using LVK.DryIoc;

namespace LVK.Syncfusion
{
    internal class RegisterSyncfusionComponentLibraries : IContainerFinalizer
    {
        [NotNull]
        private readonly IConfigurationElementWithDefault<SyncfusionLicensingConfiguration> _Configuration;

        public RegisterSyncfusionComponentLibraries([NotNull] IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _Configuration = configuration["Licensing/Syncfusion"]
               .Element<SyncfusionLicensingConfiguration>()
               .WithDefault(() => new SyncfusionLicensingConfiguration());
        }

        public void Finalize(IContainer container)
        {
            var configuration = _Configuration.Value();

            if (string.IsNullOrWhiteSpace(configuration.Key))
                return;

            global::Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(configuration.Key);
        }
    }
}