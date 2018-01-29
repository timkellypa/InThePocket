using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using InThePocket.ViewModel;

using InThePocket.Navigation;

namespace InThePocket.UI.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SongForm : PageBase
    {

        public override PageViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongFormViewModel();
                }
                return _viewModel;
            }
        }

        public SongForm() : base()
        {
            InitializeComponent();
        }
    }
}
