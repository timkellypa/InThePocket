using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;

using InThePocket.Data.Model;
using InThePocket.Data;

namespace InThePocket.ViewModel
{
    class ImportSongViewModel : PageViewModelBase
    {
        public override List<string> GetCommProperties()
        {
            return new List<string>() { };
        }

        public override Guid GetCommToken()
        {
            return Guid.Parse("8931a682-41d2-42ab-beb6-2bdd79ebe175");
        }

        public string PageTitle {
            get => $"{OriginalSongSetTitle} > Import Songs";
        }

        public ObservableCollection<SongSet> SongSetList { get; }

        public SongSet SelectedSongSet { get; set; }

        public Guid SongSetId { get; set; }

        public ObservableCollection<SongSetSong> SongList { get; }

        public string OriginalSongSetTitle { get; set; }

        public List<SongSetSong> SelectedSongs { get; }

        public ImportSongViewModel()
        {
            SongList = new ObservableCollection<SongSetSong>();
            SongSetList = new ObservableCollection<SongSet>();
            SelectedSongs = new List<SongSetSong>();

            PropertyChanged += ImportSongViewModel_PropertyChanged;
        }

        private async void ImportSongViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedSongSet" && SelectedSongSet != null)
            {
                await LoadSongs();
            }
        }

        public override async Task ProcessArguments(List<string> arguments)
        {
            string previous = null;
            bool load = false;
            arguments.ForEach((arg) =>
            {
                if (previous == "song_set_id")
                {
                    SongSetId = Guid.Parse(arg);
                }
                if (arg == "load")
                {
                    load = true;
                }
                previous = arg;
            });

            if (load)
            {
                OriginalSongSetTitle = (await DataAccess.GetSongSetById(SongSetId)).Name;
                await LoadData();
            }
        }

        private List<SongSetSong> ExistingSongSetSongList { get; set; }

        public async Task LoadData()
        {
            SelectedSongSet = null;
            SongSetList.Clear();
            List<SongSet> songSets = await DataAccess.GetSongSetList();
            foreach (SongSet songSet in songSets)
            {
                // Can't import from self
                if (songSet.Id != SongSetId)
                {
                    SongSetList.Add(songSet);
                }
            }
            ExistingSongSetSongList = await DataAccess.GetSongSetSongs(null, SongSetId);
        }

        public async Task LoadSongs()
        {
            SongList.Clear();
            List<SongSetSong> songSetSongs = await DataAccess.GetSongSetSongs(null, SelectedSongSet.Id, true);
            foreach (SongSetSong song in songSetSongs)
            {
                SongList.Add(song);
            }
        }

        public void SongSelectionChanged(IList<object> addedItems, IList<object> removedItems)
        {
            foreach (SongSetSong song in addedItems)
            {
                SelectedSongs.Add(song);
            }
            foreach (SongSetSong song in removedItems)
            {
                SelectedSongs.Remove(song);
            }
        }

        private async Task PerformSave()
        {
            foreach (SongSetSong songSetSong in SelectedSongs)
            {
                // verify that this song doesn't already exist
                bool exists = ExistingSongSetSongList.Exists((item) => item.SongId == songSetSong.SongId);
                if (!exists)
                {
                    SongSetSong newSongSetSong = new SongSetSong()
                    {
                        Notes = songSetSong.Notes,
                        SongId = songSetSong.SongId,
                        SongSetId = SongSetId,

                        // 0 will trigger model to set when saving
                        OrderNdx = 0
                    };
                    await newSongSetSong.Save();
                }
            }
        }

        private ICommand _saveClicked;
        public ICommand SaveClicked
        {
            get
            {
                if (_saveClicked == null)
                {
                    _saveClicked = new Xamarin.Forms.Command(async (sender) =>
                    {
                        await PerformSave();
                        NotifyPropertyChanged("ROUTE/Close/load");
                    });
                }
                return _saveClicked;
            }
        }
    }
}
