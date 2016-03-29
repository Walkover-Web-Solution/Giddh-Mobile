using Giddh_Cross_Portable.Interface;
using Giddh_Cross_Portable.WinPhone.useInterface;
using Giddh_Cross_Portable.WinPhone.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PageController))]

namespace Giddh_Cross_Portable.WinPhone.useInterface
{
    public class PageController : IPageController
    {
        public void GetLoginPage(NavigationPage _NavPage)
        {
            loginPage lPage = new loginPage();
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
