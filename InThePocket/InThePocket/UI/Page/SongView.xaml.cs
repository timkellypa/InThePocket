using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InThePocket.ViewModel;

using Plugin.Vibrate;

namespace InThePocket.UI.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongView : PageBase
	{
        public override PageViewModelBase ViewModel
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
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InClick" && (ViewModel as SongViewViewModel).InClick)
            {
                var v = CrossVibrate.Current;
                v.Vibration(TimeSpan.FromMilliseconds(100));
            }
        }
    }
}