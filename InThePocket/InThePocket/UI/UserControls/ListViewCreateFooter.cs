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
    public partial class ListViewCreateFooter : Grid
	{
        public static readonly BindableProperty LabelTextProperty = BindableProperty.CreateAttached(nameof(LabelText), typeof(string), typeof(ListViewCreateFooter), "");

        public string LabelText
        {
            get
            {
                return (string)GetValue(LabelTextProperty);
            }
            set
            {
                SetValue(LabelTextProperty, value);
            }
        }

        public static readonly BindableProperty CreateItemCommandProperty = BindableProperty.CreateAttached(nameof(CreateItemCommand), typeof(ICommand), typeof(ListViewCreateFooter), new Command(new Action<object>((obj) => { })));

        public ICommand CreateItemCommand
        {
            get
            {
                return (ICommand)GetValue(CreateItemCommandProperty);
            }
            set
            {
                SetValue(CreateItemCommandProperty, value);
            }
        }

        public ListViewCreateFooter()
		{
			InitializeComponent ();
		}
	}
}