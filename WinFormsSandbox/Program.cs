using System;
using System.Threading.Tasks;

using LVK.AppCore.Windows.Forms;

namespace WinFormsSandbox
{
    static class Program
    {
        [STAThread]
        static int Main() => WinFormsAppBootstrapper.RunWinFormsMainWindow<ServicesBootstrapper>(true);
    }
}
