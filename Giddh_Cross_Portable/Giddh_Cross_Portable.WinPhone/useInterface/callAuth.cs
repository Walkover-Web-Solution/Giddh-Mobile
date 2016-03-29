using Giddh_Cross_Portable.Interface;
using Giddh_Cross_Portable.WinPhone.useInterface;
using Google.Apis.Auth.OAuth2;
using Hammock.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using TweetSharp;
using Windows.Networking.Connectivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(callAuth))]

namespace Giddh_Cross_Portable.WinPhone.useInterface
{
    public class callAuth : ICallAuth
    {
        UserCredential _credential;
        public callAuth() { }
        public async void Auth()
        {
            
            if (string.IsNullOrEmpty(Constants.userObj.authKey))
            {
                //if (refreshToken == null && code == null)
                {
                    try
                    {
                        //String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id=" + Uri.EscapeDataString(Giddh_Cross_Portable.App.Instance.OAuthSettings.ClientId) + "&redirect_uri=" + Uri.EscapeDataString(Giddh_Cross_Portable.App.Instance.OAuthSettings.RedirectUrl) + "&response_type=code&scope=" + Uri.EscapeDataString(Giddh_Cross_Portable.App.Instance.OAuthSettings.Scope);
                        //System.Uri StartUri = new Uri(GoogleURL);
                        //// When using the desktop flow, the success code is displayed in the html title of this end uri
                        //System.Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval?");
                        //WebAuthenticationBroker.AuthenticateAndContinue(StartUri, EndUri, null, WebAuthenticationOptions.None);



                        _credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
                        {
                            ClientId = "641015054140-n1imq6es91te69f51mrtfbaumqb3mlqt.apps.googleusercontent.com",//"932165600137-9bdqkvf6g9naf18oddi90fg9042u3vlq.apps.googleusercontent.com",
                            ClientSecret = "wGqby6IDD9yaSWlGld0enDyy"//"UTx-_qpIBiiChi2vNhvbxiuS"
                        }, new[] { Giddh_Cross_Portable.App.Instance.OAuthSettings.Scope }, "user", CancellationToken.None);

                        Giddh_Cross_Portable.App.Instance.SaveToken(_credential.Token.AccessToken);
                        Console.WriteLine(Giddh_Cross_Portable.App.Instance.Token);
                        try
                        {
                            await _credential.RevokeTokenAsync(CancellationToken.None);
                            var response = await Giddh_Cross_Portable.App.Instance.getUserDetails();
                            if (response.status.Equals("success"))
                            {
                                Giddh_Cross_Portable.App.Instance.MainPage = Giddh_Cross_Portable.App.Instance.GetMainPage();
                            }
                            else
                            {
                                Giddh_Cross_Portable.App.Instance.broadcastProblem(response);
                            }
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    catch (Exception Error)
                    {
                        Giddh_Cross_Portable.App.Instance.broadcastInternetProblem();
                        //throw Error;
                        //throw;
                    }
                }
            }
            else
            {
                Giddh_Cross_Portable.App.Instance.MainPage = Giddh_Cross_Portable.App.Instance.GetMainPage();
            }
        }

        public async void logout()
        {
            Constants.userObj.authKey = string.Empty;
            _credential = null;
        }

        public void twitterLogin()
        {
            var service = new TwitterService(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.ClientId, Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.ClientSecret);
            GetUserProfileForOptions pfo = new GetUserProfileForOptions();
            Action<TwitterAccount, TwitterResponse> accSettings = getAccSettings;
            OAuthRequestToken rToken = new OAuthRequestToken();
            
            //service.GetAuthenticationUrl();
            //service.GetAccountSettings(accSettings);
            var requestTokenQuery = OAuthUtil.GetRequestTokenQuery();
            requestTokenQuery.RequestAsync(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.RequestTokenUrl, null);
            requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
        }

        public void getAccSettings(TwitterAccount ta, TwitterResponse tr)
        {

        }

        private void requestTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            //try
            //{
            //    StreamReader reader = new StreamReader(e.Response);
            //    string strResponse = reader.ReadToEnd();
            //    var parameters = MainUtil.GetQueryParameters(strResponse);
            //    OAuthTokenKey = parameters["oauth_token"];
            //    tokenSecret = parameters["oauth_token_secret"];
            //    var authorizeUrl = AppSettings.AuthorizeUri + "?oauth_token=" + OAuthTokenKey;

            //    Dispatcher.BeginInvoke(() =>
            //    {
            //        this.loginBrowserControl.Navigate(new Uri(authorizeUrl, UriKind.RelativeOrAbsolute));
            //    });
            //}
            //catch (Exception ex)
            //{
            //    Dispatcher.BeginInvoke(() =>
            //    {
            //        MessageBox.Show(ex.Message);
            //    });
            //}
        }
    }
}
