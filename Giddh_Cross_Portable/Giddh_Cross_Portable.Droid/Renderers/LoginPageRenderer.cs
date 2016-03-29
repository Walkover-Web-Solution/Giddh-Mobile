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
            var activity = this.Context as Activity;

            var auth = new OAuth2Authenticator(
                clientId: App.Instance.OAuthSettings.ClientId, // your OAuth2 client id
                clientSecret:App.Instance.OAuthSettings.ClientSecret,
                scope: App.Instance.OAuthSettings.Scope, // The scopes for the particular API you're accessing. The format for this will vary by API.
                authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl), // the auth URL for the service
                redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl),
                accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token"),
                getUsernameAsync: null); // the redirect URL for the service

            auth.Completed += (sender, eventArgs) => {
                if (eventArgs.IsAuthenticated)
                {
                    try
                    {
                        App.Instance.SuccessfulLoginAction.Invoke();
                        // Use eventArgs.Account to do wonderful things
                        App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                        //activity.StartActivity(typeof(ProfilePage));
                    }
                    catch(Exception ex)
                    { }
                }
                else {
                    // The user cancelled
                }
            };

            try
            {
                var intent = auth.GetUI(activity);
                //activity.StartActivity(auth.GetUI(activity));
                activity.StartActivity(intent);
            }
            catch (Exception ex)
            {

            }
        }
    }
}