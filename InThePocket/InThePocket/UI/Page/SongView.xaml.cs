using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InThePocket.ViewModel;

namespace InThePocket.UI.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongView : PageBase
	{
        public override ViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongViewViewModel();
                }
                return _viewModel;
            }
        }

        public SongView() : base()
		{
			InitializeComponent ();
		}
	}
}