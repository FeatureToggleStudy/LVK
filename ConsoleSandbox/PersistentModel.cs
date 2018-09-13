using System.ComponentModel;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace ConsoleSandbox
{
    internal class PersistentModel : INotifyPropertyChanged
    {
        private int _Count;

        public int Count
        {
            get => _Count;
            set
            {
                if (value == _Count)
                    return;

                _Count = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}