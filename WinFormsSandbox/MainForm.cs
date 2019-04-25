using System;
using System.Windows.Forms;

using JetBrains.Annotations;

using LVK.Logging;

namespace WinFormsSandbox
{
    public partial class MainForm : Form
    {
        [NotNull]
        private readonly ILogger _Logger;

        public MainForm([NotNull] ILogger logger)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            InitializeComponent();
        }

        private void Button1_Click(Object sender, EventArgs e)
        {
            _Logger.LogInformation("Test");
        }
    }
}
