using System;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;

using InThePocket.Comm;
using InThePocket.ViewModel;
using InThePocket.UI.Page;
using InThePocket.Data;

namespace InThePocket.Navigation
{
    public static class Router
    {
        const WearableBridge.Sources APP_SOURCE = WearableBridge.Sources.App;
        public static NavigationPage NavigationPage;
        public static RouterViewModel ViewModel;

        public static Page CurrentPage
        {
            get
            {
                return NavigationPage.CurrentPage;
            }
        }

        public static void Init(PageBase page)
        {
            NavigationPage = new NavigationPage(page);
            ViewModel = new RouterViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private static async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Route")
            {
                await LoadPage(ViewModel.Route);
            }
        }

        public static void Navigate(Route route)
        {
            ViewModel.Route = route;
        }

        public static async Task LoadPage(Route route)
        {
            PageBase nextPage;
            
            switch (route.Page)
            {
                case "SongSetList":
                    nextPage = new SongSetList();
                    break;
                case "SongSetListForm":
                    nextPage = new SongSetForm();
                    break;
                case "SongList":
                    nextPage = new SongList();
                    break;
                case "ImportSong":
                    nextPage = new ImportSong();
                    break;
                case "SongForm":
                    nextPage = new SongForm();
                    break;
                case "SongView":
                    nextPage = new SongView();
                    break;
                case "SongTempoForm":
                    nextPage = new SongTempoForm();
                    break;
                case "Close":
                    // Special case.  Run the pop on the main thread and re-process arguments on our view.
                    nextPage = null;
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
                    {
                        await CurrentPage.Navigation.PopAsync();
                        if (route.Arguments.Count > 0)
                        {
                            await (CurrentPage as PageBase).ProcessArguments(route.Arguments);
                        }
                    });
                    break;
                default:
                    throw new ArgumentException(string.Format("Router attempted to go to page {0}, which doesn't exist!", route.Page));
            }

            if (nextPage != null)
            {
                if (route.Arguments.Count > 0)
                {
                    await nextPage.ProcessArguments(route.Arguments);
                }

                await CurrentPage.Navigation.PushAsync(nextPage);
            }
        }
    }
}
