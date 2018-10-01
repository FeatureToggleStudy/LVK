using System;
using System.Threading;
using System.Threading.Tasks;

using JetBrains.Annotations;

using LVK.AppCore;
using LVK.Data;

namespace ConsoleSandbox
{
    public class ApplicationEntryPoint : IApplicationEntryPoint
    {
        [NotNull]
        private readonly IDatabaseConnectionFactory _DatabaseConnectionFactory;

        [NotNull]
        private readonly IDatabaseMigrator _DatabaseMigrator;

        public ApplicationEntryPoint([NotNull] IDatabaseConnectionFactory databaseConnectionFactory, [NotNull] IDatabaseMigrator databaseMigrator)
        {
            _DatabaseConnectionFactory = databaseConnectionFactory ?? throw new ArgumentNullException(nameof(databaseConnectionFactory));
            _DatabaseMigrator = databaseMigrator ?? throw new ArgumentNullException(nameof(databaseMigrator));
        }

        public async Task<int> Execute(CancellationToken cancellationToken)
        {
            using (var connection = _DatabaseConnectionFactory.Create("main").AsOpen())
            {
                await _DatabaseMigrator.Migrate(connection, "main");
                Console.WriteLine("Test");
            }

            return 0;
        }
    }
}