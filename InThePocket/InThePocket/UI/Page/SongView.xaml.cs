using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using InThePocket.ViewModel;

using Plugin.Vibrate;

using Plugin.SimpleAudioPlayer.Abstractions;
using Plugin.SimpleAudioPlayer;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace InThePocket.UI.Page
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SongView : PageBase
	{
        enum ClickType
        {
            PRIMARY,
            SECONDARY
        }
        ISimpleAudioPlayer[] Clicks = new ISimpleAudioPlayer[(int)ClickType.SECONDARY + 1];

        public override PageViewModelBase ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    _viewModel = new SongViewViewModel();
                }
                return _viewModel;
            }
        }

        public SongView() : base()
		{
            for (int i = 0; i <= (int)ClickType.SECONDARY; i++)
            {
                Clicks[i] = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
                Clicks[i].Loop = false;
            }

            InitializeComponent();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            LoadSamples();
        }

        public void LoadSamples()
        {
            Clicks[(int)ClickType.PRIMARY].Load(GetStreamFromFile("Audio.primary.wav"));
            Clicks[(int)ClickType.SECONDARY].Load(GetStreamFromFile("Audio.secondary.wav"));
        }

        Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream("InThePocket.Droid." + filename);

            return stream;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "InClick" && (ViewModel as SongViewViewModel).InClick)
            {
                SongViewViewModel vm = ViewModel as SongViewViewModel;
                switch (vm.SoundMode)
                {
                    case SongViewViewModel.SoundModes.SOUND:
                        ClickType clickType = vm.Metronome.InPrimaryClick ? ClickType.PRIMARY : ClickType.SECONDARY;
                        Clicks[(int)clickType].Play();
                        break;
                    case SongViewViewModel.SoundModes.VIBRATE:
                        var v = CrossVibrate.Current;
                        v.Vibration(TimeSpan.FromMilliseconds(100));
                        break;
                    case SongViewViewModel.SoundModes.MUTE:
                    default:
                        break;
                }

            }
        }
    }
}