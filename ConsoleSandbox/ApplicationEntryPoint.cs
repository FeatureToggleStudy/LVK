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

        public ApplicationEntryPoint([NotNull] IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _DatabaseConnectionFactory = databaseConnectionFactory ?? throw new ArgumentNullException(nameof(databaseConnectionFactory));
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            using (var connection = _DatabaseConnectionFactory.TryCreate("main"))
            {
                connection.Open();
                Console.WriteLine("Test");
            }

            return Task.FromResult(0);
        }
    }
}