using Giddh_Cross_Portable.Model;
using Giddh_Cross_Portable.WinPhone.useInterface;
using Hammock.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using TweetSharp;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.WinPhone.Views
{
    public partial class loginPage : ContentPage
    {
        string OAuthTokenKey = string.Empty;
        string tokenSecret = string.Empty;
        string accessToken = string.Empty;
        string accessTokenSecret = string.Empty;
        string userID = string.Empty;
        string userScreenName = string.Empty;

        public loginPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<Giddh_Cross_Portable.App>(this, "internet", (sender) => {
                DisplayAlert("Alert", "Check internet connection.", "OK");
            });
            MessagingCenter.Subscribe<Giddh_Cross_Portable.App, Response>(this, "problem", (sender, args) =>
            {
                DisplayAlert(args.status, args.message, "OK");
            });
        }

        public void google_Clicked(object sender, EventArgs e)
        {
            try
            {
                callAuth ca = new callAuth();
                ca.Auth();
            }
            catch (Exception ex)
            {
                DisplayAlert("Alert", "Check internet connection.","OK");
            }
        }

        public void twitter_Clicked(object sender, EventArgs e)
        {
            
            btnStack.IsVisible = false;
            loadingLabel.IsVisible = true;
            this.loginBrowserControl.IsVisible = true;

            var service = new TwitterService(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.ClientId, Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.ClientSecret);

            var requestTokenQuery = OAuthUtil.GetRequestTokenQuery();
            requestTokenQuery.RequestAsync(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.RequestTokenUrl, null);
            requestTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(requestTokenQuery_QueryResponse);
            //var authorizeUrl = Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.AuthorizeUrl + "?oauth_token=" + Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.ClientId;
            //this.loginBrowserControl.Source = authorizeUrl;
        }

        private void requestTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Response);
                string strResponse = reader.ReadToEnd();
                var parameters = MainUtil.GetQueryParameters(strResponse);
                OAuthTokenKey = parameters["oauth_token"];
                tokenSecret = parameters["oauth_token_secret"];
                var authorizeUrl = Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.AuthorizeUrl + "?oauth_token=" + OAuthTokenKey;
                Debug.WriteLine(authorizeUrl);
                //this.loginBrowserControl = new WebView()
                //{
                //    Source = new UrlWebViewSource() { Url = authorizeUrl }
                //};
                
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    Device.BeginInvokeOnMainThread(() =>
                    {                        
                        loginBrowserControl.Source = new UrlWebViewSource() { Url = authorizeUrl };
                        loadingLabel.IsVisible = false;
                    });
                });

            }
            catch (Exception ex)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        btnStack.IsVisible = true;
                        loadingLabel.IsVisible = false;
                        this.loginBrowserControl.IsVisible = false;

                    });
                });
                Debug.WriteLine(ex.Message);
            }
        }

        private void loginBrowserControl_Navigating(object sender, WebNavigationEventArgs e)
        {
            Debug.WriteLine(e.Url);
            if (e.Url.ToString().StartsWith(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.RedirectUrl))
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loadingLabel.IsVisible = true;
                    });
                });
                var AuthorizeResult = MainUtil.GetQueryParameters(e.Url.ToString());
                var VerifyPin = AuthorizeResult["oauth_verifier"];
                this.loginBrowserControl.IsVisible = false;
                var AccessTokenQuery = OAuthUtil.GetAccessTokenQuery(OAuthTokenKey, tokenSecret, VerifyPin);

                AccessTokenQuery.QueryResponse += new EventHandler<WebQueryResponseEventArgs>(AccessTokenQuery_QueryResponse);
                AccessTokenQuery.RequestAsync(Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.AccessTokenUrl, null);
            }
            else if (e.Url.ToString().Equals("https://mobile.twitter.com/login"))
            {
                var authorizeUrl = Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.AuthorizeUrl + "?oauth_token=" + OAuthTokenKey;
                Task.Run(async () =>
                {
                    await Task.Delay(3000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loginBrowserControl.Source = new UrlWebViewSource() { Url = authorizeUrl };
                        loadingLabel.IsVisible = false;
                    });
                });
            }
            else if (e.Url.ToString().Contains("https://twitter.com/login/error?username_or_email"))
            {
                var authorizeUrl = Giddh_Cross_Portable.App.Instance.twitterOAuthSettings.AuthorizeUrl + "?oauth_token=" + OAuthTokenKey;
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loginBrowserControl.Source = new UrlWebViewSource() { Url = authorizeUrl };                        
                        loadingLabel.Text = "Username and password you entered did not match.";
                        loadingLabel.IsVisible = true;
                    });
                });
                Task.Run(async () =>
                {
                    await Task.Delay(5000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loadingLabel.IsVisible = false;
                        loadingLabel.Text = "Loading please wait...";                        
                    });
                });
            }
        }

        private async void AccessTokenQuery_QueryResponse(object sender, WebQueryResponseEventArgs e)
        {
            try
            {
                StreamReader reader = new StreamReader(e.Response);
                string strResponse = reader.ReadToEnd();
                var parameters = MainUtil.GetQueryParameters(strResponse);
                accessToken = parameters["oauth_token"];
                accessTokenSecret = parameters["oauth_token_secret"];
                userID = parameters["user_id"];
                userScreenName = parameters["screen_name"];
                Giddh_Cross_Portable.App.Instance.SaveToken(accessToken);
                Giddh_Cross_Portable.App.Instance.SaveTokenSecret(accessTokenSecret);
                try
                {
                    var response = await Giddh_Cross_Portable.App.Instance.getUserDetails(true);
                    if (response.status.Equals("success"))
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(1000);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Giddh_Cross_Portable.App.Instance.MainPage = Giddh_Cross_Portable.App.Instance.GetMainPage();
                            });
                        });
                        
                    }
                    else
                    {
                        
                        Task.Run(async () =>
                        {
                            await Task.Delay(1000);
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                loadingLabel.IsVisible = false;
                                loginBrowserControl.IsVisible = false;
                                var htmlSource = new HtmlWebViewSource();
                                htmlSource.Html = @"<html><body></body></html>";
                                loginBrowserControl.Source = htmlSource;
                                btnStack.IsVisible = true;
                                DisplayAlert("Alert", response.message, "OK");
                            });
                        });
                        
                    }
                }
                catch (Exception ex)
                {
                    Task.Run(async () =>
                    {
                        await Task.Delay(1000);
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            loadingLabel.IsVisible = false;
                            loginBrowserControl.IsVisible = false;
                            var htmlSource = new HtmlWebViewSource();
                            htmlSource.Html = @"<html><body></body></html>";
                            loginBrowserControl.Source = htmlSource;
                            btnStack.IsVisible = true;
                            DisplayAlert("Alert", ex.Message, "OK");
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (!btnStack.IsVisible)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(1000);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        loadingLabel.IsVisible = false;
                        loginBrowserControl.IsVisible = false;
                        var htmlSource = new HtmlWebViewSource();
                        htmlSource.Html = @"<html><body></body></html>";
                        loginBrowserControl.Source = htmlSource;
                        btnStack.IsVisible = true;
                    });
                });
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}
