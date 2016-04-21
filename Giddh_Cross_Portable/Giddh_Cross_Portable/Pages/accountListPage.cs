using Giddh_Cross_Portable.CustomControl;
using Giddh_Cross_Portable.Model;
using Giddh_Cross_Portable.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class accountListPage : ContentPage
    {
        SearchBar searchbar = new SearchBar();
        filterListView list;
        public accountListPage()
        {
            Title = "accounts";
            list = new filterListView()
            {
                //IsGroupingEnabled = true,
                //GroupDisplayBinding = new Binding("Key"),
                //GroupShortNameBinding = new Binding("Key")
                HasUnevenRows = true,
                RowHeight = -1
            };
            searchbar = new SearchBar()
            {
                Placeholder = "Search"                
            };
                      
            //searchbar.TextChanged += (sender, e) => list.FilterLocations(searchbar.Text);
            searchbar.SearchButtonPressed += (sender, e) => {
                list.FilterLocations(searchbar.Text);
            };
            searchbar.Unfocused += (sender, e) => list.FilterLocations(searchbar.Text);
            //var sorted = from monkey in Constants.accountList orderby monkey.name group monkey by monkey.NameSort into monkeyGroup select new Grouping<string, accountDetail>(monkeyGroup.Key, monkeyGroup);
            //var MonkeysGrouped = new ObservableCollection<Grouping<string, accountDetail>>(sorted);
            list.ItemsSource = Constants.accountList.OrderBy(x => x.name).ToList();
            list.ItemTemplate = new DataTemplate(typeof(accountListCell));
            list.ItemTapped += List_ItemTapped;

            var stack = new StackLayout()
            {
                Children = {
                searchbar,
                list
            }
            };

            Content = stack;
        }

        private async void List_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            accountLedger response = new accountLedger();
            if (list.IsRefreshing)
                return;
            list.IsRefreshing = true;
            list.IsEnabled = false;
            try
            {
                
                response = await server.getAccountLedgers((accountDetail)e.Item);
                list.IsEnabled = true;
                list.IsRefreshing = false;
            }
            catch (ArgumentException aex)
            {
                list.IsEnabled = true;
                list.IsRefreshing = false;
                await DisplayAlert("Error", aex.Message, "Ok");
                return;
            }
            App.Instance.gotToLedgerPage(response);
            //var accountListPage = new accountLedgerPage(response);
            //try
            //{
            //    App.Instance.MainPage = new NavigationPage(accountListPage);
            //}
            //catch (Exception ex)
            //{ }
        }

        protected override bool OnBackButtonPressed()
        {
            var tbalanceTabPage = new trialBalanceTabbedPage();
            try
            {
                App.Instance.MainPage = new NavigationPage(tbalanceTabPage);
            }
            catch (Exception ex)
            {

            }
            return true;
        }
    }
}
