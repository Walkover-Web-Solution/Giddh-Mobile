using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.WinPhone.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(ProfilePage), typeof(ProfilePageRenderer))]

namespace Giddh_Cross_Portable.WinPhone.Renderers
{
    public class ProfilePageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);            
        }
    }
}
