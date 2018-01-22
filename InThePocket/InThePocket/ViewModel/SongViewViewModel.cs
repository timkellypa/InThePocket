using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;

using InThePocket.Tools;
using InThePocket.Data;
using InThePocket.Data.Model;

using System.Drawing;

namespace InThePocket.ViewModel
{
    public class SongViewViewModel : ViewModelBase
    {
        public Xamarin.Forms.Color PRIMARY_CLICK_UNSELECTED = new Xamarin.Forms.Color(0, 0.5, 0);
        public Xamarin.Forms.Color PRIMARY_CLICK_SELECTED = new Xamarin.Forms.Color(0, 1, 0);

        public Xamarin.Forms.Color SECONDARY_CLICK_UNSELECTED = new Xamarin.Forms.Color(0.5, 0, 0);
        public Xamarin.Forms.Color SECONDARY_CLICK_SELECTED = new Xamarin.Forms.Color(1, 0, 0);

        public override List<string> GetCommProperties()
        {
            return new List<string>() { };
        }

        public override Guid GetCommToken()
        {
             return Guid.Parse("9fb0b702-7e4f-4d95-923f-7403b09e82fb");
        }

        public string PageTitle
        {
            get => (SongSet == null || SongSetSongList == null) ? "" : $"{SongSet.Name}: Song {SongNdx} of {SongSetSongList.Count}";
        }

        public Song Model { get; set; }

        public int SongNdx { get; set; }

        public SongSet SongSet { get; set; }

        public List<SongSetSong> SongSetSongList { get; set; }

        public Metronome Metronome { get; set; }

        public List<SongTempo> SongTempoList { get; set; }

        public override async Task ProcessArguments(List<string> arguments)
        {
            string previous = null;
            bool load = false;
            Guid songSetId = Guid.Empty,
                 songId = Guid.Empty;
            arguments.ForEach((arg) =>
            {
                if (previous == "load")
                {
                    load = true;
                    songId = Guid.Parse(arg);
                }
                if (previous == "song_set_id")
                {
                    songSetId = Guid.Parse(arg);
                }
                previous = arg;
            });
            if (load)
            {
                Model = await DataAccess.GetSongById(songId);
                SongSetSongList = await DataAccess.GetSongSetSongs(null, songSetId);
                SongTempoList = await DataAccess.GetSongTemposForSong(songId);
                SongSet = await DataAccess.GetSongSetById(songSetId);
                Metronome = new Metronome(SongTempoList);
                Metronome.PropertyChanged += Metronome_PropertyChanged;
                SongNdx = SongSetSongList.FindIndex(songSetSong => songSetSong.SongId == songId) + 1;
                NotifyPropertyChanged("Model.Name");
                NotifyPropertyChanged("Model.BPM");
                NotifyPropertyChanged("Model.Notes");
                NotifyPropertyChanged("Metronome.Count");
                NotifyPropertyChanged("SongNdx");
            }
        }

        private void Metronome_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InClick")
            {
                NotifyPropertyChanged("PrimaryIndicatorBackgroundColor");
                NotifyPropertyChanged("SecondaryIndicatorBackgroundColor");
            }
        }

        private ICommand _playCommand;
        public ICommand PlayCommand
        {
            get
            {
                if (_playCommand == null)
                {
                    _playCommand = new Xamarin.Forms.Command((sender) =>
                    {
                        Metronome.Start();
                    });
                }

                return _playCommand;
            }
        }

        private ICommand _countOutCommand;
        public ICommand CountOutCommand
        {
            get
            {
                if (_countOutCommand == null)
                {
                    _countOutCommand = new Xamarin.Forms.Command((sender) =>
                    {
                        Metronome.CountOut();
                    });
                }

                return _countOutCommand;
            }
        }

        public Xamarin.Forms.Color PrimaryIndicatorBackgroundColor
        {
            get => (Metronome == null || !Metronome.InPrimaryClick) ? PRIMARY_CLICK_UNSELECTED : PRIMARY_CLICK_SELECTED;
        }

        public Xamarin.Forms.Color SecondaryIndicatorBackgroundColor
        {
            get => (Metronome == null || !Metronome.InSecondaryClick) ? SECONDARY_CLICK_UNSELECTED : SECONDARY_CLICK_SELECTED;
        }
    }
}
