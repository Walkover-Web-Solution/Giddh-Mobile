using Giddh_Cross_Portable.Helpers;
using Giddh_Cross_Portable.Interface;
using Giddh_Cross_Portable.Model;
using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable
{
    public class App : Application
    {
        // just a singleton pattern so I can have the concept of an app instance
        static volatile App _Instance;
        static object _SyncRoot = new Object();
        public static App Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new App();
                            _Instance.OAuthSettings =
                                new OAuthSettings(
                                    clientId: "641015054140-3cl9c3kh18vctdjlrt9c8v0vs85dorv2.apps.googleusercontent.com",//"932165600137-9nd18nvvkq9uqnaffkh5b41u62jmhrl1.apps.googleusercontent.com",       // your OAuth2 client id 
                                    scope: "https://www.googleapis.com/auth/userinfo.email",  		// The scopes for the particular API you're accessing. The format for this will vary by API.
                                    authorizeUrl: "https://accounts.google.com/o/oauth2/auth",  	// the auth URL for the service
                                    redirectUrl: "http://localhost:8000",
                                    clientSecret: "eWzLFEb_T9VrzFjgE40Bz6_l",//"VmX7Zrg3vavZ2tOPP4jl3DYb",
                                    refreshToken: "");   // the redirect URL for the service

                            _Instance.twitterOAuthSettings =
                                new OAuthSettings(
                                    clientId: "w64afk3ZflEsdFxd6jyB9wt5j",
                                    clientSecret: "62GfvL1A6FcSEJBPnw59pjVklVI4QqkvmA1uDEttNLbUl2ZRpy",
                                    authorizeUrl: "https://api.twitter.com/oauth/authorize",
                                    accessTokenUrl: "https://api.twitter.com/oauth/access_token",
                                    requestTokenUrl: "https://api.twitter.com/oauth/request_token"
                                    );

                            // If you'd like to know more about how to integrate with an OAuth provider, 
                            // I personally like the Instagram API docs: http://instagram.com/developer/authentication/
                        }
                    }
                }
                return _Instance;
            }            
        }
        

        public OAuthSettings OAuthSettings { get; private set; }
        public OAuthSettings twitterOAuthSettings { get; private set; }

        public NavigationPage _NavPage;

        public Page GetMainPage()
        {
            try
            {
                if (Constants.userObj == null)
                {
                    Constants.userObj = new userObject();
                    Constants.userObj.user = new userDetail();
                }
                Constants.userObj.authKey = Settings.AuthKeySettings;
                Constants.userObj.user.uniqueName = Settings.UserObjSetting;
            }
            catch (Exception ex)
            {

            }
            if (_NavPage == null)
                _NavPage = new NavigationPage();
            if (!string.IsNullOrEmpty(Constants.userObj.authKey))
            {
                ProfilePage pPage = new ProfilePage();
                _NavPage = new NavigationPage(pPage)
                {
                    BarBackgroundColor = Color.FromRgb(213, 93, 0),
                    BarTextColor = Color.FromRgb(245, 246, 239),
                    BackgroundColor = Color.FromRgb(245, 246, 239)
                };
                //_NavPage.Navigation.PushAsync(new NavigationPage(pPage)
                //{
                //    BarBackgroundColor = Color.FromRgb(213, 93, 0),
                //    BarTextColor = Color.FromRgb(245, 246, 239),
                //    BackgroundColor = Color.FromRgb(245, 246, 239)
                //});
            }
            else {
                var loginPage = new LoginPage();
                _NavPage = new NavigationPage(loginPage);
                //DependencyService.Get<IPageController>().GetLoginPage(_NavPage);
            }

            return _NavPage;
        }

        public Page GetTrialBalancePage()
        {
            var tBalancePage = new trialBalancePage();
            _NavPage = new NavigationPage(tBalancePage);
            return _NavPage;
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }

        string _Token;
        public string Token
        {
            get { return _Token; }
        }

        string _TokenSecret;
        public string TokenSecret
        {
            get { return _TokenSecret; }
        }
        public void SaveTokenSecret(string secret)
        {
            _TokenSecret = secret;
        }

        public void SaveToken(string token)
        {
            _Token = token;

            // broadcast a message that authentication was successful
            MessagingCenter.Send<App>(this, "Authenticated");
        }

        public void broadcastInternetProblem()
        {
            MessagingCenter.Send<App>(this, "internet");
        }

        public void broadcastProblem(Response resp)
        {
            MessagingCenter.Send<App,Response>(this, "problem", resp);
        }
        

        public Action SuccessfulLoginAction
        {
            get
            {                
                return new Action(() => _NavPage.Navigation.PopModalAsync());
            }
        }

        public Action LogoutAction
        {
            get
            {
                //NavigationPage prePage = _NavPage;
                Page proPage = _NavPage.CurrentPage;
                //var loginPage = new LoginPage();
                //NavigationPage prePage = new NavigationPage(loginPage);
                //_NavPage.Navigation.InsertPageBefore(prePage, proPage);
                //_NavPage.Navigation.PopToRootAsync();
                try
                {
                    DependencyService.Get<ICallAuth>().logout();
                }
                catch (Exception ex)
                {

                }
                DependencyService.Get<IPageController>().GetLoginPage(_NavPage);                

                return new Action(async () => await _NavPage.Navigation.PopToRootAsync());
            }
        }

        public Action goToProfilePage
        {
            get
            {
                Page proPage = _NavPage.CurrentPage;
                var loginPage = new ProfilePage();
                NavigationPage prePage = new NavigationPage(loginPage);
                _NavPage.Navigation.InsertPageBefore(prePage, proPage);
                //_NavPage.Navigation.PopToRootAsync();
                return new Action(async () => await _NavPage.Navigation.PopAsync());
            }
        }

        public void goToProfilePagePush()
        {
            _NavPage.Navigation.PushAsync(new ProfilePage());
        }

        public void goToTrialBalancePage()
        {
            _NavPage.Navigation.PushAsync(new trialBalanceTabbedPage());
        }

        public void gotToLedgerPage(accountLedger aLedger)
        {
            _NavPage.Navigation.PushAsync(new accountLedgerPage(aLedger));
        }

        public async Task<Response> getUserDetails(bool twitter = false)
        {
            Response resp = new Response();
            try
            {
                if (twitter)
                {
                    resp = await server.loginWithTwitter();
                }
                else
                    resp = await server.loginWithGoogle();
                if (resp.status.Equals("success"))
                {
                    Settings.UserObjSetting = Constants.userObj.user.uniqueName;
                    Settings.AuthKeySettings = Constants.userObj.authKey;
                    //helper.saveset();                   
                    ResponseA respA = await server.companies();
                }
                else
                {  
                    return resp;
                }
            }
            catch (Exception ex) {
                
            }

            return resp;
        }

        public async Task<int> getCompanies()
        {
            try
            {
                await server.companies();
            }
            catch (Exception ex)
            {
                broadcastInternetProblem();
            }
            return 1;
        }

        public async Task<int> getAccountDetails()
        {
            try
            {
                await server.getAccountsWithDetails();
            }
            catch (Exception ex)
            { }
            return 1;
        }

        public async Task<Response> getTrialBalance(DateTime from = new DateTime(), DateTime to = new DateTime())
        {
            DateTime dt = DateTime.Now;
            if (dt.Date < new DateTime(dt.Year, 4, 1))
            {
                //dt = new DateTime(dt.AddYears(-1).Year, 4, 1);
                dt = dt.AddDays(-30);
            }
            if (from.ToString().Contains("0001"))
            {
                from = dt;
                to = DateTime.Now;                
            }
            Response resp = new Response();
            try
            {
                resp = await server.getTrialBalance(from,to);
            }
            catch (Exception ex)
            {
                broadcastInternetProblem();
            }
            return resp;
        }
    }
}
