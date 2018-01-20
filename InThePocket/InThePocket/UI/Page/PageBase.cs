using System;
using System.Collections.Generic;
using InThePocket.Navigation;
using System.Threading.Tasks;
using Xamarin.Forms;
using InThePocket.ViewModel;
using InThePocket.Utils;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace InThePocket.UI.Page
{
    abstract public class PageBase : ContentPage
    {
        protected ViewModelBase _viewModel;
        abstract public ViewModelBase ViewModel { get; }

        public PageBase()
        {
            BindingContext = ViewModel;
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.StartsWith("ROUTE"))
            {
                string[] routerParts = e.PropertyName.Split('/');
                string page = routerParts[1];
                List<string> args = new List<string>();
                for (int i = 2; i < routerParts.Length; ++i)
                {
                    args.Add(routerParts[i]);
                }
                Router.Navigate(new Route(page, args));
            }
        }

        public async Task LaunchPage(PageBase page)
        {
            await Navigation.PushAsync(page);
        }

        public async Task ProcessArguments(List<string> arguments)
        {
            await ViewModel.ProcessArguments(arguments);
        }
    }
}
