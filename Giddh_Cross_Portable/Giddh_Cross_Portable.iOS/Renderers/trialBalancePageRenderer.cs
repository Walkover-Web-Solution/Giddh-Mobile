using Giddh_Cross_Portable.iOS.Renderers;
using Giddh_Cross_Portable.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(trialBalancePage), typeof(trialBalancePageRenderer))]

namespace Giddh_Cross_Portable.iOS.Renderers
{
    public class trialBalancePageRenderer : PageRenderer
    {
        UIWindow window;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            window.RootViewController = App.Instance.GetTrialBalancePage().CreateViewController();
            window.MakeKeyAndVisible();
        }
    }
}
