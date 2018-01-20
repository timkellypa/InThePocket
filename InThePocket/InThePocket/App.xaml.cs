using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Threading.Tasks;

using InThePocket.Navigation;
using System.Collections.Generic;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InThePocket
{
	public partial class App : Application
	{
        public App()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await Data.DataAccess.Init();
            }).Wait();

            Router.Init(new UI.Page.SongSetList());
            MainPage = Router.NavigationPage;
            Task.Run(async () =>
            {
                await (Router.CurrentPage as UI.Page.PageBase).ProcessArguments(new List<string>() { "load" });
            });
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
