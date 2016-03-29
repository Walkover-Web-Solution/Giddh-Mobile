using Giddh_Cross_Portable.iOS.Renderers;
using Giddh_Cross_Portable.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: ExportRenderer(typeof(ProfilePage), typeof(ProfilePageRenderer))]

namespace Giddh_Cross_Portable.iOS.Renderers
{
    public class ProfilePageRenderer : PageRenderer
    {

        UIWindow window;
        //protected override void OnElementChanged(VisualElementChangedEventArgs e)
        //{
        //    base.OnElementChanged(e);

        //    //var hostViewController = ViewController;
        //    //var viewController = (MainViewController)AppDelegate.Storyboard.InstantiateViewController("MainViewController");

        //    //hostViewController.AddChildViewController(viewController);
        //    //hostViewController.View.Add(viewController.View);

        //    //viewController.DidMoveToParentViewController(hostViewController);
        //}

        public async override void ViewDidLoad()
        {
            await App.Instance.getCompanies();
            
            base.ViewDidLoad();
        }
    }
}