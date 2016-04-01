using Giddh_Cross_Portable.Helpers;
using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class ProfilePage : BaseContentPage
    {
        ActivityIndicator act = new ActivityIndicator();
        ListView companyListView = new ListView();        
        public ProfilePage()
        {
            try
            {
                createLayout();
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Children = { act, companyListView }
                };
                MessagingCenter.Subscribe<App>(this, "internet", (sender) => {
                    DisplayAlert("Alert", "Check internet connection.", "OK");
                });
            }
            catch (Exception ex)
            {

            }
        }

        private void createLayout()
        {
            act.VerticalOptions = LayoutOptions.Center;
            act.HorizontalOptions = LayoutOptions.Center;
            act.IsVisible = false;
            Title = "Companies";
            companyListView = new ListView()
            {
                BackgroundColor = Color.FromRgb(245, 246, 239),                               
            };
            companyListView.ItemTapped += CompanyListView_ItemTapped;
            companyListView.ItemTemplate = new DataTemplate(typeof(companyCell));
            ToolbarItem logoutButton = new ToolbarItem();
            logoutButton.Text = "Logout";
            //logoutButton.Icon = @"\Assets\logout.png";
            logoutButton.Order = ToolbarItemOrder.Secondary;
            logoutButton.Clicked += LogoutButton_Clicked;
            ToolbarItems.Add(logoutButton);
            ToolbarItem refreshButton = new ToolbarItem();
            refreshButton.Text = "Refresh";
            refreshButton.Order = ToolbarItemOrder.Secondary;
            refreshButton.Clicked += RefreshButton_Clicked;
            ToolbarItems.Add(refreshButton);
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            companyListView.IsRefreshing = true;
            //if (Constants.companies == null)
            {
                await App.Instance.getCompanies();
            }
            companyListView.IsRefreshing = false;
            //act.IsVisible = false;
            //act.IsRunning = false;            
            companyListView.ItemsSource = Constants.companies;
        }

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Settings.AuthKeySettings = string.Empty;
            Settings.UserObjSetting = string.Empty;
            App.Instance.LogoutAction.Invoke();
        }

        protected async override void OnAppearing()
        {
            //act.IsVisible = true;
            //act.IsRunning = true;
            companyListView.IsRefreshing = true;
            //if (Constants.companies == null)
            {
                await App.Instance.getCompanies();                
            }
            companyListView.IsRefreshing = false;
            //act.IsVisible = false;
            //act.IsRunning = false;            
            companyListView.ItemsSource = Constants.companies;            
            base.OnAppearing();
        }

        private async void CompanyListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //act.IsVisible = true;
            //act.IsRunning = true;
            var selectedCompany = (company)e.Item;
            if (selectedCompany.sharedEntity != null)
            {
                await DisplayAlert("Alert", "You are not authorize to perform this action because you have only " + selectedCompany.sharedEntity + " permissions.", "OK");
                return;
            }
            companyListView.IsRefreshing = true;
            Response resp = new Response();
            if (Constants.selectedCompany == null || !selectedCompany.Equals(Constants.selectedCompany))
            {
                Constants.selectedCompany = selectedCompany;
                resp = await App.Instance.getTrialBalance();
                await App.Instance.getAccountDetails();
            }
            companyListView.IsRefreshing = false;
            if (resp.status == null)
            {
                return;
            }
            App.Instance.goToTrialBalancePage();
            //var accountListPage = new accountListPage();
            //try
            //{
            //    App.Instance.MainPage = new NavigationPage(accountListPage);
            //}
            //catch (Exception ex)
            //{ }
            //var tbalanceTabPage = new trialBalanceTabbedPage();
            //try
            //{
            //    App.Instance.MainPage = new NavigationPage(tbalanceTabPage)
            //    {
            //        BackgroundColor = Color.White
            //    };
            //}
            //catch (Exception ex)
            //{

            //}
        }
    }
}
