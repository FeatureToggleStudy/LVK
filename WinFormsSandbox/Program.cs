﻿using System;
using System.Threading.Tasks;

using LVK.AppCore.Windows.Forms;

namespace WinFormsSandbox
{
    static class Program
    {
        [STAThread]
        static void Main() => WinFormsAppBootstrapper.RunWinFormsMainWindow<ServicesBootstrapper>(true);
    }
}
