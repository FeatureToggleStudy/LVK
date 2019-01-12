using System;

using DryIoc;

using JetBrains.Annotations;

using LVK.DryIoc;

namespace LVK.Data.Sqlite
{
    internal class SqliteContainerFinalizer : IContainerFinalizer
    {
        [NotNull]
        private readonly ISQLitePCLInitializer _SqLitePclInitializer;

        public SqliteContainerFinalizer([NotNull] ISQLitePCLInitializer sqLitePclInitializer)
        {
            _SqLitePclInitializer = sqLitePclInitializer ?? throw new ArgumentNullException(nameof(sqLitePclInitializer));
        }

        public void Finalize(IContainer container)
        {
            _SqLitePclInitializer.InitializeOnce();
        }
    }
}