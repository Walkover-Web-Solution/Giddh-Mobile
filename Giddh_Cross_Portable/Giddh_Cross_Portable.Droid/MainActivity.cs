using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Android.Content;

namespace Giddh_Cross_Portable.Droid
{
    [Activity(Label = "Giddh Manager", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(App.Instance);
            //SetPage(App.Instance.GetMainPage());
            var intent = App.Instance.GetMainPage();
            //Intent intt = new Intent(this.BaseContext,typeof(LoginPage))
            //this.StartActivity();
            //App.Instance.MainPage = App.Instance.GetMainPage();
        }
    }
}

