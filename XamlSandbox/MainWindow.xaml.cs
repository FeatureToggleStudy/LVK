using JetBrains.Annotations;

using LVK.AppCore.Windows.Wpf;
using LVK.Mvvm;

namespace XamlSandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IApplicationEntryPointWindow
    {
        public MainWindow([NotNull] IMvvmContext context)
        {
            DataContext = new TestViewModel(context);

            InitializeComponent();
        }
    }
}