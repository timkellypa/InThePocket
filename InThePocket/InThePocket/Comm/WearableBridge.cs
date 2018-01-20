using System;
using InThePocket.ViewModel;

namespace InThePocket.Comm
{
    public static class WearableBridge
    {
        public enum Sources
        {
            App,
            Wearable
        }

        public static void SendPropertyChange(Sources source, Guid commToken, string property, object value)
        {

        }

        public static void RegisterProperyChangeHandler(ViewModelBase viewModel)
        {

        }

        public static void UnRegisterPropertyChangeHandler(ViewModelBase viewModel)
        {

        }
    }
}
