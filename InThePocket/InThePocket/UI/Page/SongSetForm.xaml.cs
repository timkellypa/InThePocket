﻿using System;
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
	public partial class SongSetForm : PageBase
	{

        public override PageViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongSetFormViewModel();
                }
                return _viewModel;
            }
        }

        public SongSetForm () : base()
		{
			InitializeComponent ();
		}
	}
}
