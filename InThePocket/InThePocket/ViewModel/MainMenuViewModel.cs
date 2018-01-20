using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;

namespace InThePocket.ViewModel
{
    public class MainMenuViewModel : ViewModelBase
    {


        public override List<String> GetCommProperties()
        {
            return new List<string>();
        }

        public override Guid GetCommToken()
        {
            return new Guid("3f58dc12-9d85-496f-8695-3c473616f205");
        }

        public override Task ProcessArguments(List<string> arguments)
        {
            throw new NotImplementedException();
        }

        private ICommand _menuItemSelected;
        public ICommand MenuItemSelected
        {
            get
            {
                if (_menuItemSelected == null)
                {
                    _menuItemSelected = new Xamarin.Forms.Command((sender) =>
                    {
                        NotifyPropertyChanged(sender as string);
                    });
                }
                return _menuItemSelected;
            }
        }
    }
}
