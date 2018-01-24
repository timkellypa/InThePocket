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

        public override ViewModelBase ViewModel
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

            Disappearing += SongForm_Disappearing;
        }

        /// <summary>
        /// If we are going back, perform a load on the previous page, because it's likely that we saved the song adding a tempo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongForm_Disappearing(object sender, EventArgs e)
        {
            if (Router.CurrentPage is SongForm)
            {
                (ViewModel as SongFormViewModel).NotifyPropertyChanged("ROUTE/IsClosing/load");
            }
        }
    }
}
