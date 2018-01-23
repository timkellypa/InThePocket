using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Syncfusion.ListView.XForms;
using InThePocket.ViewModel;

namespace InThePocket.UI.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ImportSong : PageBase
	{
        public override ViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new ImportSongViewModel();
                }
                return _viewModel;
            }
        }

        public ImportSong ()
		{
			InitializeComponent ();
		}

        private void SongSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            (ViewModel as ImportSongViewModel).SongSelectionChanged(e.AddedItems, e.RemovedItems);
        }
    }
}