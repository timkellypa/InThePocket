using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InThePocket.ViewModel;

namespace InThePocket.UI.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongSetList : PageBase
    {
        public override ViewModelBase ViewModel {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongSetListViewModel();
                }
                return _viewModel;
            }
        }

        public SongSetList() : base()
        {
			InitializeComponent();
        }

        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
            => ((ListView)sender).SelectedItem = null;
    }
}
