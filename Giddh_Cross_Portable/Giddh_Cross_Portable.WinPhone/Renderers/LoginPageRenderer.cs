using Giddh_Cross_Portable.Model;
using Giddh_Cross_Portable.Pages;
using Giddh_Cross_Portable.WinPhone.Renderers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]

namespace Giddh_Cross_Portable.WinPhone.Renderers
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {            
            base.OnElementChanged(e);
        }
    }
}
