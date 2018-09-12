using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using JetBrains.Annotations;

using LVK.Core.Services;
using LVK.Logging;

namespace LVK.AppCore.Tray
{
    internal class TrayIconBackgroundService : IBackgroundService
    {
        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        [NotNull]
        private readonly ILogger _Logger;

        [NotNull, ItemNotNull]
        private readonly List<ITrayIconMenuItem> _MenuItems;

        [CanBeNull]
        private Thread _Thread;

        public TrayIconBackgroundService(
            [NotNull] IEnumerable<ITrayIconMenuItem> menuItems,
            [NotNull] IApplicationLifetimeManager applicationLifetimeManager, [NotNull] ILogger logger)
        {
            if (menuItems == null)
                throw new ArgumentNullException(nameof(menuItems));

            _ApplicationLifetimeManager = applicationLifetimeManager
                                       ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));

            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _MenuItems = menuItems.ToList();
        }

        public Task Start(CancellationToken cancellationToken)
        {
            var thread = new Thread(BackgroundThread);
            thread.Start();
            _Thread = thread;
            
            return Task.CompletedTask;
        }

        private void BackgroundThread()
        {
            var contextMenu = new ContextMenu();

            bool isFirst = true;
            foreach (var menuItem in _MenuItems.OrderBy(mi => mi.Order))
            {
                var item = contextMenu.MenuItems.Add(menuItem.Caption);
                item.Break = menuItem.BeginGroup && !isFirst;
                isFirst = false;

                item.Click += (sender, args) => menuItem.Execute();
                item.Popup += (sender, args) =>
                {
                    item.Enabled = menuItem.Enabled;
                    item.Visible = menuItem.Visible;
                    item.Text = menuItem.Caption;
                };
            }

            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            using (new NotifyIcon
            {
                Text = assembly.GetName().Name,
                Icon = Icon.ExtractAssociatedIcon(assembly.Location),
                Visible = true,
                ContextMenu = contextMenu
            })
            {
                var context = new ApplicationContext();
                _ApplicationLifetimeManager.GracefulTerminationCancellationToken.Register(Application.Exit);
 
                using (_Logger.LogScope(LogLevel.Debug, "Tray icon message loop"))
                    Application.Run(context);
            }
        }

        public async Task Stop(CancellationToken cancellationToken)
        {
            await Task.Run(() => _Thread?.Join(), cancellationToken);
        }
    }
}