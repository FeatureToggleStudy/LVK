using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using JetBrains.Annotations;

using LVK.Mvvm;
using LVK.Mvvm.Properties;
using LVK.Mvvm.Properties.ListProperties;
using LVK.Mvvm.Properties.Signals;
using LVK.Mvvm.Wpf.ViewModels;

namespace XamlSandbox
{
    internal class TestViewModel : WpfViewModel
    {
        [NotNull]
        private readonly ObservableCollectionProperty<string> _CurrentFilesOnDisk;

        [NotNull]
        private ISignal _UpdateSignal;

        public TestViewModel([NotNull] IMvvmContext context)
            : base(context)
        {
            _UpdateSignal = Signal();
            var currentFilesOnDiskSource = AsyncComputed(GetCurrentFilesOnDisk);
            _CurrentFilesOnDisk = new ObservableCollectionProperty<string>(Context, currentFilesOnDiskSource);
            Update = WpfCommand(nameof(Update), ExecuteUpdate);
        }

        public ICommand Update { get; }

        private void ExecuteUpdate()
        {
            _UpdateSignal.Pulse();
        }

        public ObservableCollection<string> FilesOnDisk => _CurrentFilesOnDisk;

        private async Task<string[]> GetCurrentFilesOnDisk(CancellationToken ct)
        {
            _UpdateSignal.RegisterDependency();

            await Task.Yield();
            return Directory.GetFiles(@"C:\Dev", "*.*", SearchOption.AllDirectories);
        }
    }
}