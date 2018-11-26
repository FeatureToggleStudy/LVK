using JetBrains.Annotations;

namespace LVK.AppCore.Windows.Tray
{
    [PublicAPI]
    public interface ITrayIconMenuItem
    {
        int Order { get; }
        bool BeginGroup { get; }
        
        [NotNull]
        string Caption { get; }

        bool Visible { get; }
        bool Enabled { get; }

        void Execute();
    }
}