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

namespace LVK.AppCore.Tray
{
    internal class TrayIconBackgroundService : IBackgroundService
    {
        [NotNull, ItemNotNull]
        private readonly List<ITrayIconMenuItem> _MenuItems;

        [CanBeNull]
        private NotifyIcon _TrayIcon;

        public TrayIconBackgroundService([NotNull] IEnumerable<ITrayIconMenuItem> menuItems)
        {
            if (menuItems == null)
                throw new ArgumentNullException(nameof(menuItems));

            _MenuItems = menuItems.ToList();
        }

        public Task Start(CancellationToken cancellationToken)
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
            _TrayIcon = new NotifyIcon
            {
                Text = assembly.GetName().Name,
                Icon = Icon.ExtractAssociatedIcon(assembly.Location),
                Visible = true,
                ContextMenu = contextMenu
            };

            return Task.CompletedTask;
        }

        public Task Stop(CancellationToken cancellationToken)
        {
            if (_TrayIcon != null)
            {
                _TrayIcon.Visible = false;
                _TrayIcon.Dispose();
            }

            _TrayIcon = null;
            return Task.CompletedTask;
        }
    }
}