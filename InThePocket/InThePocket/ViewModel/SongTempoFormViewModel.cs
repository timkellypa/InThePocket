using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Linq;

using InThePocket.Data;
using InThePocket.Data.Model;

namespace InThePocket.ViewModel
{
    class SongTempoFormViewModel : ViewModelBase
    {
        private const int BEATS_PER_BAR_MIN = 2;
        private const int BEATS_PER_BAR_MAX = 32;

        private int[] BEAT_UNITS = new int[] { 2, 4, 8, 16, 32 };
        private int[] ACCENT_BEATS_PER_BAR_OPTIONS = new int[] { 1, 2 };
        private string[] ACCENT_BEATS_PER_BAR_DESC = new string[] { "1 - accent beat on 1", "2 - accent beat on 1 and in middle of bar" };

        private List<KeyValuePair<int, string>> _beatsPerBarOptions;
        public List<KeyValuePair<int, string>> BeatsPerBarOptions
        {
            get
            {
                if (_beatsPerBarOptions == null)
                {
                    _beatsPerBarOptions = new List<KeyValuePair<int, string>>();
                    for (int i = BEATS_PER_BAR_MIN; i <= BEATS_PER_BAR_MAX; ++i)
                    {
                        _beatsPerBarOptions.Add(new KeyValuePair<int, string>(i, i.ToString()));
                    }
                }
                return _beatsPerBarOptions;
            }
        }

        private List<KeyValuePair<int, string>> _beatUnitOptions;
        public List<KeyValuePair<int, string>> BeatUnitOptions
        {
            get
            {
                if (_beatUnitOptions == null)
                {
                    _beatUnitOptions = new List<KeyValuePair<int, string>>();
                    for (int i = 0; i < BEAT_UNITS.Length; ++i)
                    {
                        _beatUnitOptions.Add(new KeyValuePair<int, string>(BEAT_UNITS[i], BEAT_UNITS[i].ToString()));
                    }
                }
                return _beatUnitOptions;
            }
        }

        private List<KeyValuePair<int, string>> _accentBeatsPerBarOptions;
        public List<KeyValuePair<int, string>> AccentBeatsPerBarOptions
        {
            get
            {
                if (_accentBeatsPerBarOptions == null)
                {
                    _accentBeatsPerBarOptions = new List<KeyValuePair<int, string>>();
                    for (int i = 0; i < ACCENT_BEATS_PER_BAR_OPTIONS.Length; ++i)
                    {
                        string desc = ACCENT_BEATS_PER_BAR_OPTIONS[i].ToString();
                        if (i < ACCENT_BEATS_PER_BAR_DESC.Length)
                        {
                            desc = ACCENT_BEATS_PER_BAR_DESC[i];
                        }
                        _accentBeatsPerBarOptions.Add(new KeyValuePair<int, string>(ACCENT_BEATS_PER_BAR_OPTIONS[i], desc));
                    }
                }
                return _accentBeatsPerBarOptions;
            }
        }

        public KeyValuePair<int, string> SelectedBeatsPerBar
        {
            get
            {
                return BeatsPerBarOptions.Find(item => item.Key == Model.BeatsPerBar);
            }
            set
            {
                Model.BeatsPerBar = value.Key;
            }
        }

        public KeyValuePair<int, string> SelectedBeatUnit
        {
            get
            {
                return BeatUnitOptions.Find(item => item.Key == Model.BeatUnit);
            }
            set
            {
                Model.BeatUnit = value.Key;
            }
        }

        public KeyValuePair<int, string> SelectedAccentBeatsPerBar
        {
            get
            {
                return AccentBeatsPerBarOptions.Find(item => item.Key == Model.AccentBeatsPerBar);
            }
            set
            {
                Model.AccentBeatsPerBar = value.Key;
            }
        }

        public override List<string> GetCommProperties()
        {
            return new List<string> { };
        }

        public override Guid GetCommToken()
        {
            return Guid.Parse("8f7a295d-d340-426a-96ce-fcb54be49278");
        }

        public SongTempoFormViewModel()
        {
            Model = new SongTempo();
        }

        public override async Task ProcessArguments(List<string> arguments)
        {
            string previous = null;
            arguments.ForEach((arg) =>
            {
                if (previous == "edit")
                {
                    Edit = Guid.Parse(arg);
                }
                if (arg == "add")
                {
                    Add = true;
                    Edit = null;
                }
                if (previous == "song_id")
                {
                    SongId = Guid.Parse(arg);
                }
                previous = arg;
            });
            if (Edit.HasValue)
            {
                Model = await DataAccess.GetSongTempoById(Edit.Value);
                SongId = Model.SongId;
                NotifyPropertyChanged("Model.DisplayText");
            }
            else
            {
                Model.SongId = SongId;
            }
        }

        public bool Add { get; set; }

        public Guid? Edit { get; set; }

        public SongTempo Model { get; set; }

        public Guid SongId { get; set; }

        public string PageTitle
        {
            get => Add ? "New Tempo" : $"Edit Tempo: {Model.DisplayText}";
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
                            await Model.Save();
                            NotifyPropertyChanged("ROUTE/Close/load");
                        });
                    });
                }

                return _saveClicked;
            }
        }
    }
}
