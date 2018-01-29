using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using InThePocket.DependencyServices;

using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(InThePocket.Droid.DependencyServices.CloseApplication))]

namespace InThePocket.Droid.DependencyServices
{
    class CloseApplication : ICloseApplication
    {
        public CloseApplication() { }

        public void Close()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}