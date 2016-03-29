using CoreGraphics;
using Foundation;
using Giddh_Cross_Portable.iOS.Renderers;
using Giddh_Cross_Portable.Pages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(trialBalanceCarouselPage), typeof(trialBalanceCarouselPageRenderer))]

namespace Giddh_Cross_Portable.iOS.Renderers
{
    public class trialBalanceCarouselPageRenderer : CarouselPageRenderer
    {
        public override void ViewDidAppear(bool animated)
        {
            
            this.NavigationController.NavigationBarHidden = true;
            UIBarButtonItem[] listUiButton = new UIBarButtonItem[] {
                new UIBarButtonItem("From", UIBarButtonItemStyle.Plain, (s,e) => {
                      Console.WriteLine("From clicked");
                }),
                new UIBarButtonItem("To", UIBarButtonItemStyle.Plain, (s,e) => {
                    Console.WriteLine ("To clicked");
                }),
                new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (s,e) => {
                    Console.WriteLine("Refresh clicked");
                })                
            };

            UIToolbar uitb = new UIToolbar()
            {
                Items = listUiButton,
                Frame = new CGRect(0, this.View.Bounds.Height - 44, this.View.Bounds.Width, 44)
            };

            UIDatePicker fromDate;
            fromDate = new UIDatePicker();
            fromDate.Mode = UIDatePickerMode.Date;
            fromDate.MaximumDate = NSDate.Now;
            fromDate.TimeZone = NSTimeZone.LocalTimeZone;
            fromDate.UserInteractionEnabled = true;
            fromDate.BackgroundColor = UIColor.White;
            fromDate.Hidden = true;

            UIDatePicker toDate;
            toDate = new UIDatePicker();
            toDate.Mode = UIDatePickerMode.Date;
            toDate.MaximumDate = NSDate.Now;
            toDate.TimeZone = NSTimeZone.LocalTimeZone;
            toDate.UserInteractionEnabled = true;
            toDate.BackgroundColor = UIColor.White;
            toDate.Hidden = true;


            UIButton reloadButton = new UIButton();
            reloadButton.SetTitle("reload", UIControlState.Normal);
            reloadButton.SetTitleColor(UIColor.LightGray, UIControlState.Disabled);
            UIButton fromDateButton = new UIButton();
            fromDateButton.SetTitle("from", UIControlState.Normal);            
            fromDateButton.SetTitleColor(UIColor.LightGray, UIControlState.Disabled);
            UIButton toDateButton = new UIButton();
            toDateButton.SetTitle("to", UIControlState.Normal);
            toDateButton.SetTitleColor(UIColor.LightGray, UIControlState.Disabled);


            try
            {
                //UIStackView dateStackView = new UIStackView();
                //dateStackView.Axis = UILayoutConstraintAxis.Horizontal;
                //dateStackView.Distribution = UIStackViewDistribution.FillEqually;
                //dateStackView.AddSubview(fromDate);
                //dateStackView.AddSubview(toDate);
                //dateStackView.AddSubview(reloadButton);
                UIView pDateView;
                pDateView = new UIView(new CGRect(0, this.View.Bounds.Height - 100, this.View.Bounds.Width, 100));
                pDateView.AddSubview(fromDateButton);
                pDateView.AddSubview(toDateButton);
                pDateView.AddSubview(reloadButton);

                //this.NavigationController.View.AddSubview(uitb);            
                //this.NativeView.AddSubview(uitb);
                //this.NativeView.AddSubview(pDateView);
                //this.NativeView.AddSubview(fromDate);
            }

            catch (Exception ex)
            {
                
            }
            //this.NavigationController.SetToolbarItems(listUiButton, false);
            //this.NavigationController.SetToolbarHidden(false, false);
            base.ViewDidAppear(animated);
        }

        private void ApplyButton_Clicked(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ViewWillAppear(bool animated)
        {            
            base.ViewWillAppear(animated);
        }        
    }
}
