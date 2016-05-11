using GiddhDesktop.Common;
using GiddhDesktop.Common.Modal;
using GiddhDesktop.Common.Services;
using GiddhDesktop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GiddhDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class companyPage : Page
    {
        public companyPage()
        {
            this.InitializeComponent();
            getCompanies();
        }

        public async void getCompanies()
        {
            await server.companies();
            setCompanies();
        }

        public void setCompanies()
        {
            this.companyComboBox.ItemsSource = Constants.companies;
            if (Constants.companies.Count > 1)
            {
                this.companyComboBox.SelectedIndex = 0;                
            }
        }

        public async void getAccounts(bool hitApi = true)
        {
            if (hitApi)
            {
                showProgressRing(true);
                await server.getAccountsWithDetails();
                showProgressRing(false);
            }
            groupAccounts(Constants.accountList);
            accountSection.DataContext = Constants.groupAccountList;
        }

        public void groupAccounts(ObservableCollection<accountDetail> accountList)
        {
            var accounts = accountList.GroupBy(x => x.parentGroupUniqueName).Select(g => new
            {
                uName = g.Key,
                name = g.Select(x => x.parentGroupName).ToString(),
                accList = g.ToList()
            });
            Constants.groupAccountList = new ObservableCollection<accountWithParent>();
            foreach (var account in accounts)
            {
                accountWithParent acc = new accountWithParent();
                acc.uniqueName = account.uName;
                acc.name = account.accList[0].parentGroupName;
                acc.accountList = new ObservableCollection<accountDetail>(account.accList);
                Constants.groupAccountList.Add(acc);
            }
        }

        private void companyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Constants.selectedCompany = (company)this.companyComboBox.SelectedItem;
            getAccounts();
        }

        private void searchAccount_Event(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string filter = args.QueryText;
            //if (string.IsNullOrEmpty(args.QueryText))
            {
                if (string.IsNullOrWhiteSpace(filter))
                {
                    var aList = Constants.accountList.OrderBy(x => x.parentGroupName).ToList();
                    groupAccounts(new ObservableCollection<accountDetail>(aList));
                }
                else
                {
                    var aList = Constants.accountList
                        .Where(x => x.name.ToLower()
                       .Contains(filter.ToLower()) || x.uniqueName.ToLower().Contains(filter.ToLower()) || x.parentGroupName.ToLower().Contains(filter.ToLower()) || x.parentGroupUniqueName.ToLower().Contains(filter.ToLower())).OrderBy(x => x.parentGroupName).ToList();
                    groupAccounts(new ObservableCollection<accountDetail>(aList));
                }
                accountSection.DataContext = Constants.groupAccountList;
            }
        }

        public void showProgressRing(bool show)
        {            
            //if (show)
            //{
            //    this.accountProgressRing.Visisbility = Visibility.Visible;
            //    this.accountListView.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    this.accountProgressRing.Visisbility = Visibility.Collapsed;
            //    this.accountListView.Visibility = Visibility.Visible;
            //}
        }

        private async void getLedger_Event(object sender, ItemClickEventArgs e)
        {
            accountLedger al = await server.getAccountLedgers((accountDetail)e.ClickedItem);
        }

        private void accountStackTapped_Event(object sender, TappedRoutedEventArgs e)
        {
            StackPanel s = sender as StackPanel;
            Constants.selectedAccount = (accountDetail)s.DataContext;
            this.MainFrame.Navigate(typeof(accountLedgerPage), Constants.selectedAccount);
        }

        private async void goButton_CLick(object sender, RoutedEventArgs e)
        {
            
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }
    }
    
}
