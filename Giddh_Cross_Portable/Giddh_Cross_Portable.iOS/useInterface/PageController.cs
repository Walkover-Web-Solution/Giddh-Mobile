using Giddh_Cross_Portable.Interface;
using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.iOS.useInterface;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(PageController))]

namespace Giddh_Cross_Portable.iOS.useInterface
{
    public class PageController : IPageController
    {
        public void GetLoginPage(NavigationPage _NavPage)
        {
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
