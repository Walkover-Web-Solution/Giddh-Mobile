using Giddh_Cross_Portable.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class LoginPage : ContentPage
    {
        public LoginPage()
        {
            //Content = new Button
            //{
            //    Text = "LOGIN",
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Center                
            //};

            Button btn = new Button
            {
                Text = "Google",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                BorderWidth = 0,
                BackgroundColor = Color.FromRgb(211, 72, 54),
                BorderRadius = 5
            };
            Button twitterBtn = new Button
            {
                Text = "Twitter",
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderWidth = 1,
                BackgroundColor = Color.FromRgb(211, 72, 54)
            };
            twitterBtn.Clicked += TwitterBtn_Clicked;
            btn.Clicked += Btn_Clicked;            
            
            StackLayout stc = new StackLayout();
            stc.Orientation = StackOrientation.Vertical;
            stc.HorizontalOptions = LayoutOptions.CenterAndExpand;
            stc.VerticalOptions = LayoutOptions.CenterAndExpand;
            stc.Children.Add(btn);
            //stc.Children.Add(twitterBtn);
            Content = stc;
        }

        private void TwitterBtn_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<ICallAuth>().twitterLogin();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //DependencyService.Get<ICallAuth>().Auth();
        }



        private async void Btn_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Alert", "login button click", "OK");
            try
            {
				DependencyService.Get<ICallAuth>().Auth(this);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Alert", "Please check your internet connection.", "OK");
            }
        }
    }
}
