using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

using System.Linq;

using InThePocket.Data;

using InThePocket.Data.Model;

namespace InThePocket.ViewModel
{
    class SongListViewModel : ViewModelBase
    {
        public override Guid GetCommToken()
        {
            return new Guid("e22ef27d-5961-4380-b495-923d7991f9c5");
        }

        public override List<string> GetCommProperties()
        {
            return new List<string> { "Items" };
        }

        public Guid? SongSetID { get; set; }

        public override async Task ProcessArguments(List<string> arguments)
        {
            bool nextIsID = false,
                load = false;
            foreach (string arg in arguments)
            {
                if (nextIsID)
                {
                    SongSetID = Guid.Parse(arg);
                    nextIsID = false;
                    continue;
                }

                switch (arg)
                {
                    case "load":
                        load = true;
                        break;
                    case "song_set_id":
                        nextIsID = true;
                        break;
                }
            }
            if (load)
            {
                await RefreshData();
            }
        }

        public ObservableCollection<SongSetSong> Items { get; }

        public SongListViewModel()
        {
            Items = new ObservableCollection<SongSetSong>();

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public ICommand RefreshDataCommand { get; }

        async Task RefreshData()
        {
            IsBusy = true;
            Items.Clear();

            List<SongSetSong> songSetSongList = await DataAccess.GetSongSetSongs(null, SongSetID, true);
            foreach (var songSetSong in songSetSongList)
            {
                Items.Add(songSetSong);
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
                        NotifyPropertyChanged($"ROUTE/SongForm/add/song_set_id/{SongSetID.ToString()}");
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
                        NotifyPropertyChanged($"ROUTE/SongForm/edit/{(sender as SongSetSong).SongId.ToString()}/song_set_id/{SongSetID.ToString()}");
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
                        await (sender as SongSetSong).Delete();
                        Items.Remove(sender as SongSetSong);
                        IsBusy = false;
                    });
                }
                return _delete;
            }
        }

        private SongSetSong _selectedItem;

        public SongSetSong SelectedItem
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
                        NotifyPropertyChanged($"ROUTE/SongView/load/{value.SongId.ToString()}/song_set_id/{SongSetID.ToString()}");
                    }
                }
            }
        }
    }
}
