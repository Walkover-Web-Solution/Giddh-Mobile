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
using Giddh_Cross_Portable.Droid.useInterface;
using Giddh_Cross_Portable.Interface;
using Xamarin.Forms;
using Giddh_Cross_Portable.Pages;

[assembly: Dependency(typeof(PageController))]


namespace Giddh_Cross_Portable.Droid.useInterface
{
    public class PageController : IPageController
    {
        public void GetLoginPage(NavigationPage _NavPage)
        {
            //var loginPage = new LoginPage();
            //_NavPage = new NavigationPage(loginPage);
            LoginPage lPage = new LoginPage();
            if (_NavPage.Navigation.NavigationStack.Count > 0)
            {
                _NavPage.Navigation.InsertPageBefore(new NavigationPage(lPage), _NavPage.CurrentPage);
            }
            else
                _NavPage.Navigation.PushAsync(new NavigationPage(lPage));
        }

        public void GetProfilePage(NavigationPage _NavPage)
        {
            //profilePage pPage = new profilePage();
            //_NavPage.Navigation.PushAsync(pPage);
        }

        public void GetTrialBalancePage(NavigationPage _NavPage)
        { }
    }
}