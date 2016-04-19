using Giddh_Cross_Portable.Interface;
using Giddh_Cross_Portable.iOS.Renderers;
using Giddh_Cross_Portable.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
[assembly: Dependency(typeof(callAuth))]

namespace Giddh_Cross_Portable.iOS.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        bool IsShown;
        OAuth2Authenticator auth;
        UIWindow window;

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (!IsShown) {

                IsShown = true;
                callGoogle();
            }
        }

        public void callGoogle()
        {
            //auth = new OAuth2Authenticator(
            //    clientId: App.Instance.OAuthSettings.ClientId, // your OAuth2 client id
            //    clientSecret: App.Instance.OAuthSettings.ClientSecret,
            //    scope: App.Instance.OAuthSettings.Scope, // The scopes for the particular API you're accessing. The format for this will vary by API.
            //    authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl), // the auth URL for the service
            //    redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl),
            //    accessTokenUrl: new Uri("https://accounts.google.com/o/oauth2/token")
            //    ); // the redirect URL for the service
            auth = new OAuth2Authenticator(clientId: "641015054140-5p4laf3lbjvda9bmi94rvcs6m9q13q5v.apps.googleusercontent.com",
            scope: "https://www.googleapis.com/auth/userinfo.email",
            authorizeUrl: new Uri("https://accounts.google.com/o/oauth2/auth"),
            redirectUrl: new Uri("https://www.googleapis.com/plus/v1/people/me"),
            getUsernameAsync: null);
            auth.ClearCookiesBeforeLogin = true;
            auth.AllowCancel = true;
            auth.ShowUIErrors = false;
            auth.Title = "Giddh";
            auth.BrowsingCompleted += Auth_BrowsingCompleted;
            
            auth.Completed += Auth_Completed;
            try
            {
                PresentViewController(auth.GetUI(), true, null);
            }
            catch (Exception ex)
            {

            }
        }

        private void Auth_BrowsingCompleted(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }

        private async void Auth_Completed(object sender, AuthenticatorCompletedEventArgs eventArgs)
        {            
            if (eventArgs.IsAuthenticated)
            {
                //App.Instance.SuccessfulLoginAction.Invoke();
                // Use eventArgs.Account to do wonderful things
                Console.WriteLine(eventArgs.Account.Properties["access_token"]);
                App.Instance.SaveToken(eventArgs.Account.Properties["access_token"]);
                Console.WriteLine(App.Instance.Token);
                try
                {
                    var response = await App.Instance.getUserDetails();
                    if (!response.status.Equals("success"))
                    {
                        //redirect to profile page from here
                        //var pPage = App.Instance.GetMainPage().CreateViewController();
                        //NavigationController.PushViewController(pPage, true);
                        App.Instance.broadcastProblem(response);


                    }

                    window = new UIWindow(UIScreen.MainScreen.Bounds);

                    window.RootViewController = App.Instance.GetMainPage().CreateViewController();
                    window.MakeKeyAndVisible();

                }
                catch (Exception ex)
                {

                }                

            }
            else {
                // The user cancelled
                
            }            
        }
    }

    public class callAuth : ICallAuth
    {
        public async void Auth(object getThis)
        {
            //OAuth2Authenticator auth;
            LoginPageRenderer lpr = new LoginPageRenderer();
            lpr.callGoogle();
        }

        public void twitterLogin()
        {

        }

        public void logout()
        {
            Constants.userObj.authKey = string.Empty;
        }
    }
}
