using Android.App;
using Android.Content.PM;
using Android.OS;

using Android.Views;

using Android.Support.V7.Widget;

using System.Linq;
using InThePocket.UI.Page;

namespace InThePocket.Droid
{
    [Activity(Label = "InThePocket.Android", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());

            Toolbar toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // check if the current item id 
            // is equals to the back button id
            if (item.ItemId == 16908332)
            {
                // retrieve the current xamarin forms page instance
                var currentPage = (PageBase)
                Xamarin.Forms.Application.
                Current.MainPage.Navigation.
                NavigationStack.LastOrDefault();

                return currentPage.BackButtonPressed();
            }
            else
            {
                // since its not the back button 
                //click, pass the event to the base
                return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            // this is not necessary, but in Android user 
            // has both Nav bar back button and
            // physical back button its safe 
            // to cover the both events

            // retrieve the current xamarin forms page instance
            var currentPage = (PageBase)
            Xamarin.Forms.Application.
            Current.MainPage.Navigation.
            NavigationStack.LastOrDefault();

            currentPage.BackButtonPressed();
        }
    }
}