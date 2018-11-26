using System.Windows;

using LVK.AppCore.Windows.Wpf;

namespace XamlSandbox
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            WpfAppBootstrapper.RunWpfMainWindow<ServicesBootstrapper>();
        }
    }
}