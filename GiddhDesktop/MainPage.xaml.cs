using GiddhDesktop.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GiddhDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        const string googleClientId = "641015054140-n1imq6es91te69f51mrtfbaumqb3mlqt.apps.googleusercontent.com";
        const string googleClientSecret = "wGqby6IDD9yaSWlGld0enDyy";
        const string googleScope = "https://www.googleapis.com/auth/userinfo.email";
        const string googleRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        private async void googleClick_Event(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                
                String GoogleURL = "https://accounts.google.com/o/oauth2/auth?client_id=" +
                    Uri.EscapeDataString(googleClientId) +
                    "&redirect_uri=" + Uri.EscapeDataString(googleRedirectUri) +
                    "&response_type=code&scope=" + Uri.EscapeDataString(googleScope);

                Uri StartUri = new Uri(GoogleURL);
                // When using the desktop flow, the success code is displayed in the html title of this end uri
                Uri EndUri = new Uri("https://accounts.google.com/o/oauth2/approval?");

                //rootPage.NotifyUser("Navigating to: " + GoogleURL, NotifyType.StatusMessage);

                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, StartUri, EndUri);
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    //googleButton.IsEnabled = false;
                    
                    btn.IsEnabled = false;
                    var code = GetCode(WebAuthenticationResult.ResponseData);
                    var serviceRequest = await GetToken(code);
                    var response = await server.loginWithGoogle(serviceRequest.access_token);
                    if (response.status.ToLower().Equals("success"))
                    {
                        btn.IsEnabled = true;
                        this.Frame.Navigate(typeof(companyPage));
                        //var dialog = new MessageDialog("Your message here");
                        //await dialog.ShowAsync();
                    }
                    else
                    {
                        btn.IsEnabled = true;
                        var dialog = new MessageDialog(response.message,response.status);
                        await dialog.ShowAsync();
                    }
                    //OutputToken(WebAuthenticationResult.ResponseData.ToString());
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                    btn.IsEnabled = true;
                    // OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                    btn.IsEnabled = true;
                    //OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                btn.IsEnabled = true;
                //rootPage.NotifyUser(Error.Message, NotifyType.ErrorMessage);
            }
        }

        private static async Task<ServiceResponse> GetToken(string code)
        {
            const string TokenUrl = "https://accounts.google.com/o/oauth2/token  ";
            var body = new StringBuilder();
            body.Append(code);
            body.Append("&client_id=");
            body.Append(Uri.EscapeDataString(googleClientId));
            body.Append("&client_secret=");
            body.Append(Uri.EscapeDataString(googleClientSecret));
            body.Append("&redirect_uri=");
            body.Append(Uri.EscapeDataString(googleRedirectUri));
            body.Append("&grant_type=authorization_code");
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(TokenUrl))
            {
                Content = new StringContent(body.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var serviceTequest = JsonConvert.DeserializeObject<ServiceResponse>(content);
            return serviceTequest;
        }

        private string GetCode(string webAuthResultResponseData)
        {
            // Success code=4/izytpEU6PjuO5KKPNWSB4LK3FU1c
            var split = webAuthResultResponseData.Split(' ');
            return split.FirstOrDefault(value => value.Contains("code"));
        }
    }

    public class ServiceResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
    }
}
