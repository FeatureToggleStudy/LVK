using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using DryIoc;

using JetBrains.Annotations;

using LVK.Core.Services;

namespace LVK.AppCore.Windows.Forms
{
    public class WinFormsApplicationEntryPoint<TMainForm> : IApplicationEntryPoint
        where TMainForm: Form
    {
        [NotNull]
        private readonly IResolver _Resolver;

        [NotNull]
        private readonly IApplicationLifetimeManager _ApplicationLifetimeManager;

        public WinFormsApplicationEntryPoint([NotNull] IResolver resolver, [NotNull] IApplicationLifetimeManager applicationLifetimeManager)
        {
            _Resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _ApplicationLifetimeManager = applicationLifetimeManager ?? throw new ArgumentNullException(nameof(applicationLifetimeManager));
        }

        public Task<int> Execute(CancellationToken cancellationToken)
        {
            Form mainForm = _Resolver.Resolve<TMainForm>();

            bool closingHasBeenHandled = true;
            _ApplicationLifetimeManager.GracefulTerminationCancellationToken.Register(
                () =>
                {
                    if (closingHasBeenHandled)
                        return;

                    closingHasBeenHandled = true;
                    mainForm.Close();
                });

            mainForm.FormClosed += (s, e) =>
            {
                closingHasBeenHandled = true;
                _ApplicationLifetimeManager.SignalGracefulTermination();
            };

            Application.Run(mainForm);
            return Task.FromResult(0);
        }
    }
}
