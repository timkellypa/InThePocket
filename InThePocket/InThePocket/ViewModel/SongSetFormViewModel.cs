using System;
using System.Collections.Generic;

using System.Threading.Tasks;

using InThePocket.Data.Model;
using InThePocket.Data;
using System.Windows.Input;

namespace InThePocket.ViewModel
{
    class SongSetFormViewModel : PageViewModelBase
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
            get => Add ? "New SetList" : $"{Model.Name} > Edit Setlist";
        }

        public bool Add { get; set; }

        public Guid? Edit { get; set; }

        public SongSet Model { get; set; }

        public SongSetFormViewModel()
        {
            Model = new SongSet();
        }

        public override async Task ProcessArguments(List<string> arguments)
        {
            bool nextIsID = false;
            arguments.ForEach((arg) =>
            {
                if (nextIsID)
                {
                    Edit = Guid.Parse(arg);
                }
                nextIsID = false;
                if (arg == "add")
                {
                    Add = true;
                    Edit = null;
                }
                if (arg == "edit")
                {
                    nextIsID = true;
                }
            });
            if (Edit.HasValue)
            {
                Model = await DataAccess.GetSongSetById(Edit.Value);
                NotifyPropertyChanged("Model.Name");
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
