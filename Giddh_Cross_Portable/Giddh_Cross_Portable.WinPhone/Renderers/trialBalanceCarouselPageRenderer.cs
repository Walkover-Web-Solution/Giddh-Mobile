using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.WinPhone.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(trialBalanceCarouselPage), typeof(trialBalanceCarouselPageRenderer))]

namespace Giddh_Cross_Portable.WinPhone.Renderers
{
    public class trialBalanceCarouselPageRenderer : CarouselPageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }
    }
}
