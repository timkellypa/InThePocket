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
    public partial class SongList : PageBase
    {
        public override PageViewModelBase ViewModel {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongListViewModel();
                }
                return _viewModel;
            }
        }

        public SongList() : base()
        {
			InitializeComponent();
        }
    }
}
