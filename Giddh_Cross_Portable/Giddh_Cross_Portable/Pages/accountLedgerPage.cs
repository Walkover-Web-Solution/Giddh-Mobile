using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    class accountLedgerPage : ContentPage
    {
        accountLedger accntLedger = new accountLedger();
        ListView transactionList;
        public accountLedgerPage(accountLedger acntLedger)
        {
            accntLedger = acntLedger;
            this.BindingContext = accntLedger;
            var dateWiseList = accntLedger.ledgers.GroupBy(x => x.entryDate).Select(g => new { entryDate = g.Key, ledgers = g.ToList() }).ToList();
            //ObservableCollection<datewiseLedger> dwiseLedger = new ObservableCollection<datewiseLedger>();
            ObservableCollection<ledgerFinal> ledgerList = new ObservableCollection<ledgerFinal>();
            foreach (var dList in dateWiseList)
            {
                //var aList = new datewiseLedger(dList.entryDate);
                var cLedger = new ledgerFinal(dList.entryDate);
                cLedger.shortDate = dList.entryDate.Remove(dList.entryDate.LastIndexOf("-"));
                //aList.ledgers = new ObservableCollection<ledger>(dList.ledgers);
                //aList.transactions = new ObservableCollection<showLedger>();
                foreach (ledger ld in dList.ledgers)
                {                    
                    foreach (transaction tr in ld.transactions)
                    {                        
                        showLedger sl = new showLedger();
                        sl.name = tr.particular.name;
                        if (tr.type.ToLower().Equals("credit"))
                        {
                            sl.creditAmount = tr.amount;
                        }
                        if (tr.type.ToLower().Equals("debit"))
                        {
                            sl.debitAmount = tr.amount;
                        }
                        cLedger.Add(sl);
                        //aList.transactions.Add(sl);
                    }
                    
                }
                //dwiseLedger.Add(aList);
                ledgerList.Add(cLedger);
            }
            ledgerList = new ObservableCollection<ledgerFinal>(ledgerList.Reverse());
            //dwiseLedger = new ObservableCollection<datewiseLedger>(dwiseLedger.Reverse());
            transactionList = new ListView()
            {
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Date"),
                GroupShortNameBinding = new Binding("shortDate"),
                HasUnevenRows = true,
                RowHeight = -1,
                ItemsSource = ledgerList,
                ItemTemplate = new DataTemplate(typeof(aLedgerInsideCell)),
                GroupHeaderTemplate = new DataTemplate(typeof(ledgerGroupHeaderCell))
            };
        }

        protected override void OnAppearing()
        {
            
            Title = accntLedger.acDetail.name;
            Label companyLabel = new Label()
            {
                FontSize = 32
            };
            companyLabel.Text = accntLedger.acDetail.name;
            Label balanceLabel = new Label();
            balanceLabel.BindingContextChanged += BalanceLabel_BindingContextChanged;
            balanceLabel.BindingContext = accntLedger.balance;
            
            StackLayout headerStack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { new Label { Text = "Particular", HorizontalOptions = LayoutOptions.StartAndExpand, WidthRequest = 160 }, new Label { Text = "Dr.", HorizontalOptions = LayoutOptions.EndAndExpand, WidthRequest = 80, HorizontalTextAlignment = TextAlignment.End }, new Label { Text = "Cr.", HorizontalOptions = LayoutOptions.EndAndExpand, WidthRequest = 80, HorizontalTextAlignment = TextAlignment.End } }
            };

            Content = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(10),
                Children = { companyLabel, balanceLabel, headerStack, transactionList }
            };
            base.OnAppearing();
        }

        private void TransactionList_BindingContextChanged(object sender, EventArgs e)
        {
            
        }

        private void BalanceLabel_BindingContextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            balance bal = (balance)lb.BindingContext;
            lb.Text = "Balance : " + bal.amount.ToString("N").Replace(".00", "");
            if (bal.type.ToLower().Equals("credit"))
            {
                lb.Text += " Cr.";
            }
            else
            {
                lb.Text += "Dr.";
            }
        }

        private void createTransactionList()
        {
            Grid insideGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1,GridUnitType.Star) },
                }
            };
        }

        protected override bool OnBackButtonPressed()
        {
            if (Constants.selectedCompany.sharedEntity == null)
            {
                App.Instance.MainPage = new NavigationPage(new trialBalanceTabbedPage());
                //App.Instance._NavPage.Navigation.PopAsync();
                return true;
            }
            else
            {
                App.Instance.MainPage = new NavigationPage(new accountListPage());
                //App.Instance._NavPage.Navigation.PopAsync();
                return true;
            }
            return base.OnBackButtonPressed();
        }
    }
}
