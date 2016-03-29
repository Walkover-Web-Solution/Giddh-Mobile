using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Interface
{
    public interface IPageController
    {
        void GetLoginPage(NavigationPage _NavPage);
        void GetProfilePage(NavigationPage _NavPage);
        void GetTrialBalancePage(NavigationPage _NavPage);
    }
}
