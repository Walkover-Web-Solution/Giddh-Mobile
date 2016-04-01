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
using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

//[assembly: ExportRenderer(typeof(trialBalanceTabbedPage), typeof(trialBalanceTabbedPageRenderer))]

namespace Giddh_Cross_Portable.Droid.Renderers
{
    class trialBalanceTabbedPageRenderer : TabbedPageRenderer
    {
        public override void OnViewAdded(Android.Views.View child)
        {            
            base.OnViewAdded(child);
        }
    }
}