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
using Xamarin.Forms;
using Giddh_Cross_Portable.Droid.Renderers;
using Xamarin.Forms.Platform.Android;

//[assembly: ExportRenderer(typeof(ContentPage), typeof(customContentRenderer))]

namespace Giddh_Cross_Portable.Droid.Renderers
{
    public class customContentRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            RemoveAppIconFromActionBar();
        }

        void RemoveAppIconFromActionBar()
        {
            var actionBar = ((Activity)Context).ActionBar;
            actionBar.Hide();
        }
    }
}