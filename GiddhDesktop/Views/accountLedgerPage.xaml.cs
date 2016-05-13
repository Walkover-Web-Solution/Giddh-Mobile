using GiddhDesktop.Common;
using GiddhDesktop.Common.Modal;
using GiddhDesktop.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GiddhDesktop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class accountLedgerPage : Page
    {
        accountDetail acDetail = new accountDetail();

        public accountLedgerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            fromDatePicker.Date = new DateTimeOffset(DateTime.Now.AddMonths(-1));
            toDatePicker.Date = new DateTimeOffset(DateTime.Now);
            var account = (accountDetail)e.Parameter;
            acDetail = account;
            getLedger(account);            
        }

        public async void getLedger(accountDetail acDetail)
        {
            accountLedger al = await server.getAccountLedgers(acDetail,fromDatePicker.Date.Value.ToString("dd-MM-yyyy"),toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
            mainGrid.DataContext = al;
        }

        private void goButton_Click(object sender, RoutedEventArgs e)
        {
            getLedger(acDetail);
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string filter = sender.Text;
                if (string.IsNullOrWhiteSpace(filter))
                {
                    sender.ItemsSource = new List<accountDetail>();
                    //var aList = Constants.accountList.OrderBy(x => x.parentGroupName).ToList();
                    //groupAccounts(new ObservableCollection<accountDetail>(aList));
                }
                else
                {
                    var aList = Constants.accountList
                        .Where(x => (x.name.ToLower()
                       .Contains(filter.ToLower()) || x.uniqueName.ToLower().Contains(filter.ToLower())) && !x.uniqueName.ToLower().Equals(acDetail.uniqueName)).OrderBy(x => x.name).ToList();
                    sender.ItemsSource = aList;
                }
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //sender.Text = ((accountDetail)args.SelectedItem).name;
            sender.DataContext = (accountDetail)args.SelectedItem;
        }
    }
}
