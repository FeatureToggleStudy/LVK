using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using JetBrains.Annotations;

using LVK.Mvvm.Properties;

namespace LVK.Mvvm.ViewModels
{
    public partial class ViewModel : INotifyPropertyChanged
    {
        [NotNull, ItemNotNull]
        private readonly HashSet<string> _WrittenPropertyNames = new HashSet<string>();

        public event PropertyChangedEventHandler PropertyChanged;
 
        private void ContextOnPropertyWriteScopeExited(object sender, EventArgs e)
        {
            var propertyNames = _WrittenPropertyNames.ToList();
            _WrittenPropertyNames.Clear();

            var evt = PropertyChanged;
            if (evt is null)
                return;

            foreach (var propertyName in propertyNames)
                evt(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RegisterWrite(IProperty property)
        {
            if (!_PropertyNames.TryGetValue(property, out string propertyName))
                return;

            _WrittenPropertyNames.Add(propertyName);
        }
    }
}