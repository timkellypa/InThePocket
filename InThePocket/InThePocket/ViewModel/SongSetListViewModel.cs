using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using InThePocket.Data;

using InThePocket.Data.Model;

namespace InThePocket.ViewModel
{
    class SongSetListViewModel : PageViewModelBase
    {
        public override Guid GetCommToken()
        {
            return new Guid("b7e5b35f-d963-4749-812b-73af4c3cb2ff");
        }

        public override List<string> GetCommProperties()
        {
            return new List<string> { "Items" };
        }

        public override async Task ProcessArguments(List<string> arguments)
        {
            foreach (string arg in arguments)
            {
                if (arg == "load")
                {
                    await RefreshData();
                }
            }
        }

        private SongSet _selectedItem;
        public SongSet SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    NotifyPropertyChanged("SelectedItem");
                    if (_selectedItem != null)
                    {
                        Task.Run(async () => {
                            await Task.Delay(250);
                            SelectedItem = null;
                        });
                        NotifyPropertyChanged(string.Format("ROUTE/SongList/load/song_set_id/{0}", value.Id));
                    }
                }
            }
        }

        public ObservableCollection<SongSet> Items { get; }

        public SongSetListViewModel()
        {
            Items = new ObservableCollection<SongSet>();

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            Items.Clear();

            List<SongSet> songList = await DataAccess.GetSongSetList();

            foreach (var song in songList)
            {
                Items.Add(song);
            }

            IsBusy = false;
        }

        bool busy;
        public bool IsBusy
        {
            get { return busy; }
            set
            {
                busy = value;
                NotifyPropertyChanged("IsBusy");
                ((Command)RefreshDataCommand).ChangeCanExecute();
            }
        }

        private ICommand _createItem;
        public ICommand CreateItem
        {
            get
            {
                if (_createItem == null)
                {
                    _createItem = new Xamarin.Forms.Command((sender) =>
                    {
                        NotifyPropertyChanged("ROUTE/SongSetListForm/add");
                    });
                }
                return _createItem;
            }
        }

        private ICommand _edit;
        public ICommand Edit
        {
            get
            {
                if (_edit == null)
                {
                    _edit = new Xamarin.Forms.Command((sender) =>
                    {
                        NotifyPropertyChanged("ROUTE/SongSetListForm/edit/" + (sender as SongSet).Id.ToString());
                    });
                }
                return _edit;
            }
        }

        private ICommand _delete;
        public ICommand Delete
        {
            get
            {
                if (_delete == null)
                {
                    _delete = new Xamarin.Forms.Command(async (sender) =>
                    {
                        IsBusy = true;
                        await (sender as SongSet).Delete();
                        Items.Remove(sender as SongSet);
                        IsBusy = false;
                    });
                }
                return _delete;
            }
        }

    }

}
