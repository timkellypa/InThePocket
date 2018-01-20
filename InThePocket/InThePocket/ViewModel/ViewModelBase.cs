using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using InThePocket.Comm;

using InThePocket.Utils;

using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace InThePocket.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public abstract Guid GetCommToken();

        public abstract List<String> GetCommProperties();

        public abstract Task ProcessArguments(List<string> arguments);

#pragma warning disable 0414
        private WearableBridge.Sources _source;
#pragma warning restore 0414

        public ViewModelBase() : base()
        {
            _source = WearableBridge.Sources.App;
        }

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
