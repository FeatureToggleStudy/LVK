using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Files
{
    [PublicAPI]
    public class ServicesBootstrapper : IServicesBootstrapper
    {
        public void Bootstrap(IContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            container.Bootstrap<Logging.ServicesBootstrapper>();

            container.Register<IFileCopier, FileCopier>();
            container.Register<IFileMover, FileMover>();
            container.Register<IStreamComparer, StreamComparer>();
            container.Register<IFileServices, FileServices>();
            container.Register<IFileContentsCopier, FileContentsCopier>();
        }
    }
}