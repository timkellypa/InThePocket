using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

using System.Threading.Tasks;
using System.Linq;

using InThePocket.Data.Model;
using InThePocket.Data;
using System.Windows.Input;

namespace InThePocket.ViewModel
{
    class SongFormViewModel : ViewModelBase
    {
        public override List<string> GetCommProperties()
        {
            return new List<string>() { "Add", "Edit", "Name" };
        }

        public override Guid GetCommToken()
        {
            return new Guid("e98c5851-1ca3-40e9-a487-f91e8aba03e1");
        }

        public string PageTitle
        {
            get => Add ? "New Song" : $"Edit Song: {Model.Name}";
        }

        public bool Add { get; set; }

        public Guid? Edit { get; set; }

        public Guid SongSetId { get; set; }

        public Guid SongSetSongId { get; set; }

        private SongSetSong EditingSongSetSong { get; set; }

        public Song Model { get; set; }

        private string _newNotes;
        public string Notes
        {
            get
            {
                if (EditingSongSetSong != null)
                {
                    return EditingSongSetSong.Notes;
                }
                return _newNotes;
            }
            set
            {
                if (EditingSongSetSong != null)
                {
                    EditingSongSetSong.Notes = value;
                }
                _newNotes = value;
            }
        }
        public ObservableCollection<SongTempo> SongTempos { get; set; }
        
        public SongFormViewModel()
        {
            Model = new Song();

            SongTempos = new ObservableCollection<SongTempo>();

            RefreshDataCommand = new Command(
                async () => await RefreshData());
        }

        public override async Task ProcessArguments(List<string> arguments)
        {
            string previous = null;
            bool load = false;
            arguments.ForEach((arg) =>
            {
                if (previous == "edit")
                {
                    Edit = Guid.Parse(arg);
                    load = true;
                }
                if (arg == "add")
                {
                    Add = true;
                    Edit = null;
                }
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
            if (Edit.HasValue)
            {
                EditingSongSetSong = (await DataAccess.GetSongSetSongs(Edit.Value, SongSetId)).First();
                Model = await DataAccess.GetSongById(Edit.Value);
                NotifyPropertyChanged("Model.Name");
            }

            if (load)
            {
                await RefreshData();
            }
        }

        public ICommand RefreshDataCommand { get; }

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

        async Task RefreshData()
        {
            IsBusy = true;
            SongTempos.Clear();

            List<SongTempo> tempoList = await DataAccess.GetSongTemposForSong(Model.Id);

            foreach (var tempo in tempoList)
            {
                SongTempos.Add(tempo);
            }

            IsBusy = false;
        }

        private ICommand _createTempo;
        public ICommand CreateTempo
        {
            get
            {
                if (_createTempo == null)
                {
                    _createTempo = new Xamarin.Forms.Command(async (sender) =>
                    {
                        await PerformSave();
                        NotifyPropertyChanged($"ROUTE/SongTempoForm/add/song_id/{Model.Id}");
                    });
                }
                return _createTempo;
            }
        }

        private ICommand _editTempo;
        public ICommand EditTempo
        {
            get
            {
                if (_editTempo == null)
                {
                    _editTempo = new Xamarin.Forms.Command((sender) =>
                    {
                        NotifyPropertyChanged($"ROUTE/SongTempoForm/edit/{(sender as SongTempo).Id}/song_id/{Model.Id}");
                    });
                }
                return _editTempo;
            }
        }

        private ICommand _deleteTempo;
        public ICommand DeleteTempo
        {
            get
            {
                if (_deleteTempo == null)
                {
                    _deleteTempo = new Xamarin.Forms.Command(async (sender) =>
                    {
                        await (sender as SongTempo).Delete();
                        SongTempos.Remove(sender as SongTempo);
                    });
                }
                return _deleteTempo;
            }
        }

        public async Task PerformSave()
        {
            await Model.Save();
            if (Add)
            {
                SongSetSong newSongSetSong = new SongSetSong()
                {
                    Id = Guid.NewGuid(),
                    SongId = Model.Id,
                    SongSetId = SongSetId,
                    Notes = Notes,

                    // 0 will trigger model to set when saving
                    OrderNdx = 0
                };
                await newSongSetSong.Save();

                // from now on, set up this control to edit, not add
                Edit = Model.Id;
                Add = false;
                await ProcessArguments(new List<string>() { "load" });
            }
            else
            {
                await EditingSongSetSong.Save();
                await Model.Save();
            }
        }

        private ICommand _saveClicked;
        public ICommand SaveClicked
        {
            get
            {
                if (_saveClicked == null)
                {
                    _saveClicked = new Xamarin.Forms.Command((sender) =>
                    {
                        Task.Run(async () =>
                        {
                            await PerformSave();
                            NotifyPropertyChanged("ROUTE/Close/load");
                        });
                    });
                }
                return _saveClicked;
            }
        }
    }
}
