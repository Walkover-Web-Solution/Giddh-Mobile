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
using Giddh_Cross_Portable.Pages;
using Xamarin.Forms;
using Giddh_Cross_Portable.Droid.Renderers;
using Xamarin.Auth;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace Giddh_Cross_Portable.Droid.Renderers
{
    class LoginPageRenderer : PageRenderer
    {     
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            try
            {
                LoginByGoogle(false);
            }
            catch (AggregateException ex)
            { }
        }

        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (!e.IsAuthenticated)
            {
                //Toast.MakeText(this, "Fail to authenticate!", ToastLength.Short).Show();
                return;
            }
            string access_token;
            e.Account.Properties.TryGetValue("access_token", out access_token);
            //App.Instance.SuccessfulLoginAction.Invoke();
            // Use eventArgs.Account to do wonderful things
            Console.WriteLine(access_token);
            App.Instance.SaveToken(access_token);
            Console.WriteLine(App.Instance.Token);
            try
            {
                var response = await App.Instance.getUserDetails();
                if (response.status.Equals("success"))
                {
                    //redirect to profile page from here
                    //var pPage = App.Instance.GetMainPage().CreateViewController();
                    //NavigationController.PushViewController(pPage, true);
                    App.Instance.goToProfilePagePush();
                }
            }
            catch (Exception ex)
            {

            }
        }

        void LoginByGoogle(bool allowCancel)
        {
            var auth = new OAuth2Authenticator(clientId: "641015054140-5p4laf3lbjvda9bmi94rvcs6m9q13q5v.apps.googleusercontent.com",
            scope: "https://www.googleapis.com/auth/userinfo.email",
            authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
            redirectUrl: new Uri("https://www.googleapis.com/plus/v1/people/me"),
            getUsernameAsync: null);
            auth.AllowCancel = allowCancel;
            auth.ClearCookiesBeforeLogin = true;
            auth.Completed += Auth_Completed;

            var activity = this.Context as Activity;
            var intent = auth.GetUI(this.Context);
            activity.StartActivity(intent);
        }
    }
}