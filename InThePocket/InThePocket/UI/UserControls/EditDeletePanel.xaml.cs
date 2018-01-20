using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InThePocket.UI.UserControls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EditDeletePanel : Grid
	{
        public static readonly BindableProperty EditCommandProperty = BindableProperty.CreateAttached(nameof(EditCommand), typeof(ICommand), typeof(EditDeletePanel), new Command(new Action<object>((obj) => { })));
        public ICommand EditCommand
        {
            get
            {
                return (ICommand)GetValue(EditCommandProperty);
            }
            set
            {
                SetValue(EditCommandProperty, value);
            }
        }

        public static readonly BindableProperty DeleteCommandProperty = BindableProperty.CreateAttached(nameof(DeleteCommand), typeof(ICommand), typeof(EditDeletePanel), new Command(new Action<object>((obj) => { })));
        public ICommand DeleteCommand
        {
            get
            {
                return (ICommand)GetValue(DeleteCommandProperty);
            }
            set
            {
                SetValue(DeleteCommandProperty, value);
            }
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.CreateAttached(nameof(CommandParameter), typeof(object), typeof(EditDeletePanel), null);
        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }

        public EditDeletePanel ()
		{
			InitializeComponent ();
		}
	}
}