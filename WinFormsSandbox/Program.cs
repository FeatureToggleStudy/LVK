using System;
using System.Threading.Tasks;

using LVK.AppCore.Windows.Forms;

namespace WinFormsSandbox
{
    static class Program
    {
        [STAThread]
        static Task<int> Main() => WinFormsAppBootstrapper.RunWinFormsMainWindowAsync<ServicesBootstrapper>(true);
    }
}
