using GiddhDesktop.Common;
using GiddhDesktop.Common.Modal;
using GiddhDesktop.Common.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Notifications;
using Windows.UI.Popups;
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
        /*{       name: "Sales"       shortCode: "sal"     }     {       name: "Purchases"       shortCode: "pur"     }     {       name: "Receipt"       shortCode: "rcpt"     }     {       name: "Payment"       shortCode: "pay"     }     {       name: "Journal"       shortCode: "jr"     }     {       name: "Contra"       shortCode: "cntr"     }     {       name: "Debit Note"       shortCode: "debit note"     }     {       name: "Credit Note"       shortCode: "credit note"     } */
        List<KeyValuePair<string, string>> voucherList = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Sales","sal"),
            new KeyValuePair<string, string>("Purchases","pur"),
            new KeyValuePair<string, string>("Receipt","rcpt"),
            new KeyValuePair<string, string>("Payment","pay"),
            new KeyValuePair<string, string>("Journal","jr"),
            new KeyValuePair<string, string>("Contra","cntr"),
            new KeyValuePair<string, string>("Debit Note","debit note"),
            new KeyValuePair<string, string>("Credit Note","credit note")
        };
        accountDetail acDetail = new accountDetail();
        company companySelected = new company();
        string inCaseOfUpdate = string.Empty;
        ObservableCollection<ledgerTransaction> ledgerTransactionss = new ObservableCollection<ledgerTransaction>();
        accountLedger al = new accountLedger();

        #region download file
        DownloadOperation downloadOperation;
        CancellationTokenSource cancellationToken;
        BackgroundDownloader backgroundDownloader = new BackgroundDownloader();
        #endregion

        public accountLedgerPage()
        {
            this.InitializeComponent();
            companySelected = Constants.selectedCompany;
        }

        public void createTransactionList()
        {

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
            al = await server.getAccountLedgers(acDetail,fromDatePicker.Date.Value.ToString("dd-MM-yyyy"),toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
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
            if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
            {
                var alist = Constants.accountList.Where(x => x.uniqueName.ToLower().Equals(sender.Text.ToLower())).ToList();
                sender.ItemsSource = alist;
                sender.IsSuggestionListOpen = true;
                //sender.Text = alist[0].name;
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            //sender.Text = ((accountDetail)args.SelectedItem).name;
            sender.DataContext = (accountDetail)args.SelectedItem;
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            clearDetails();      
        }

        public void clearDetails(bool wantToFocus = true)
        {
            inCaseOfUpdate = string.Empty;
            debitDate.Date = new DateTimeOffset(DateTime.Now);
            debitParticular.Text = "";
            debitAmount.Text = "";
            voucherTypeCombo.SelectedIndex = 0;
            tagTextBox.Text = "";
            descriptionBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            if (wantToFocus)
                debitDate.Focus(FocusState.Keyboard);
            ledgerTransactionss.Clear();
        }

        private async void createEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Constants.permissionAllowed)
            {
                showToastNotification("You don't have sufficient permission to add new entry.");
                return;
            }
            ledgerToSend lts = new ledgerToSend();

            if (ledgerTransactionss.Count <= 0)
            {
                if (string.IsNullOrEmpty(debitParticular.Text))
                {
                    return;
                }
                ledgerTransaction lt = new ledgerTransaction();
                lt.particular = ((accountDetail)debitParticular.DataContext).uniqueName;
                lt.amount = debitAmount.Text;
                lt.type = typeCombo.SelectionBoxItem.ToString();
                lts.transactions = new ledgerTransaction[] { lt };
            }
            else
            {
                if (string.IsNullOrEmpty(debitParticular.Text))
                {
                    ledgerTransaction lt = new ledgerTransaction();
                    lt.particular = ((accountDetail)debitParticular.DataContext).uniqueName;
                    lt.amount = debitAmount.Text;
                    lt.type = typeCombo.SelectionBoxItem.ToString();
                    ledgerTransactionss.Add(lt);
                }
                lts.transactions = ledgerTransactionss.ToArray();
            }
            lts.entryDate = debitDate.Date.Value.ToString("dd-MM-yyyy");
            string description = "";
            descriptionBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out description);
            lts.description = description;
            lts.voucherType = (voucherList.Where(x => x.Key.Equals(voucherTypeCombo.SelectionBoxItem.ToString())).ToList())[0].Value.ToString();
            lts.tag = tagTextBox.Text;
            Response response = await server.createNewEntry(lts,inCaseOfUpdate);
            if (response.status.ToLower().Equals("success"))
            {
                getLedger(acDetail);
                clearButton_Click(sender, e);
            }
            else
            { showToastNotification(response.message); }
        }

        private async void sendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string emails = string.Empty;
            emailBox.Document.GetText(Windows.UI.Text.TextGetOptions.UseObjectText, out emails);
            if (string.IsNullOrEmpty(emails))
            {
                showToastNotification("Enter atleast one email address");                
                emailBox.Focus(FocusState.Keyboard);
                return;
            }
            var emailArray = emails.Split(',');
            foreach (string str in emailArray)
            {
                if (!isValidEmail(str))
                {
                    showToastNotification(str + " is not valid email address");                    
                    emailBox.Focus(FocusState.Keyboard);
                    return;
                }
            }
            Response response = await server.mailLedger(emailArray.Select(x=> x).Distinct().ToArray(), fromDatePicker.Date.Value.ToString("dd-MM-yyyy"), toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
            //if (response.status.ToLower().Equals("success"))
            {
                emailBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                showToastNotification(response.message);
            }
        }

        #region download file
        private async void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            Response response = await server.exportLedger(fromDatePicker.Date.Value.ToString("dd-MM-yyyy"), toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
            string filePath = response.body["filePath"].ToString();
            Download(filePath,"ledger.xls");
        }

        
        public async void Download(string urlToDownload,string fileName)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            folderPicker.ViewMode = PickerViewMode.Thumbnail;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageFile file = await folder.CreateFileAsync(acDetail.name+fileName, CreationCollisionOption.GenerateUniqueName);
                Uri durl = new Uri(urlToDownload);
                downloadOperation = backgroundDownloader.CreateDownload(durl, file);
                Progress<DownloadOperation> progress = new Progress<DownloadOperation>(progressChanged);
                cancellationToken = new CancellationTokenSource();

                try
                {
                    Statustext.Text = "Initializing...";
                    await downloadOperation.StartAsync().AsTask(cancellationToken.Token, progress);
                }
                catch (TaskCanceledException)
                {

                    downloadOperation.ResultFile.DeleteAsync();
                    downloadOperation = null;
                }
            }
        }
        private void progressChanged(DownloadOperation downloadOperation)
        {
            int progress = (int)(100 * ((double)downloadOperation.Progress.BytesReceived / (double)downloadOperation.Progress.TotalBytesToReceive));
            Statustext.Text = String.Format("{0} of {1} kb. downloaded - %{2} complete.", downloadOperation.Progress.BytesReceived / 1024, downloadOperation.Progress.TotalBytesToReceive / 1024, progress);

            switch (downloadOperation.Progress.Status)
            {
                case BackgroundTransferStatus.Running:
                    {
                        break;
                    }
                case BackgroundTransferStatus.PausedByApplication:
                    {

                        break;
                    }
                case BackgroundTransferStatus.PausedCostedNetwork:
                    {

                        break;
                    }
                case BackgroundTransferStatus.PausedNoNetwork:
                    {

                        break;
                    }
                case BackgroundTransferStatus.Error:
                    {
                        Statustext.Text = "An error occured while downloading.";
                        break;
                    }
            }
            if (progress >= 100)
            {
                downloadOperation = null;
                Statustext.Text = "";
            }
        }
        #endregion

        private void addInDr_Click(object sender, RoutedEventArgs e)
        {
            ledgerTransaction lt = new ledgerTransaction();
            lt.particular = ((accountDetail)debitParticular.DataContext).uniqueName;
            lt.amount = debitAmount.Text;
            lt.type = typeCombo.SelectionBoxItem.ToString();
            ledgerTransactionss.Add(lt);
            typeCombo.SelectedIndex = 0;
            debitParticular.Text = "";
            debitAmount.Text = "";
            debitParticular.Focus(FocusState.Pointer);
        }

        private void addInCr_Click(object sender, RoutedEventArgs e)
        {
            ledgerTransaction lt = new ledgerTransaction();
            lt.particular = ((accountDetail)debitParticular.DataContext).uniqueName;
            lt.amount = debitAmount.Text;
            lt.type = typeCombo.SelectionBoxItem.ToString();
            ledgerTransactionss.Add(lt);
            typeCombo.SelectedIndex = 1;
            debitParticular.Text = "";
            debitAmount.Text = "";
            debitParticular.Focus(FocusState.Pointer);
        }

        private void debitDate_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                if (args.NewDate.Value > new DateTimeOffset(DateTime.Now))
                {
                    sender.Date = new DateTimeOffset(DateTime.Now);
                    showToastNotification("You can't make future entry");
                }
            }
            catch
            { }
        }

        public void showToastNotification(string message)
        {
            var toast = new ToastNotification(Constants.CreateToast(message));
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toast);
        }

        private void deleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            ledgerTransaction lt = (ledgerTransaction)(sender as AppBarButton).DataContext;
            ledgerTransactionss.Remove(lt);
        }

        private void multipleEntryItem_Click(object sender, ItemClickEventArgs e)
        {
            ledgerTransaction lt = (ledgerTransaction)e.ClickedItem;
            ledgerTransactionss.Remove(lt);
            debitAmount.Text = lt.amount;
            debitParticular.Text = lt.particular;
            if (lt.type.ToLower().Equals("credit"))
            {
                typeCombo.SelectedIndex = 1;
            }
            else
                typeCombo.SelectedIndex = 0;
        }        

        private void editTransaction_Click(object sender, RoutedEventArgs e)
        {
            ledgerTransaction lt = (ledgerTransaction)((sender as AppBarButton).DataContext);
            ledgerTransactionss.Remove(lt);
            debitAmount.Text = lt.amount;
            debitParticular.Text = lt.particular;
            if (lt.type.ToLower().Equals("credit"))
            {
                typeCombo.SelectedIndex = 1;
            }
            else
                typeCombo.SelectedIndex = 0;
        }

        private void updateEntry_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!Constants.permissionAllowed)
            {
                showToastNotification("You don't have sufficient permission to edit entry.");
                return;
            }
            transaction t = (transaction)(sender as StackPanel).DataContext;
            if (t.invoiceGenerated)
            {
                showToastNotification("Invoice generated for ledger unable to edit.");
                return;
            }
            string[] dates = t.entryDate.Split('-');
            int[] dateInt = { System.Convert.ToInt32(dates[2]), System.Convert.ToInt32(dates[1]), System.Convert.ToInt32(dates[0]) };
            DateTime dt = new DateTime(dateInt[0], dateInt[1], dateInt[2]);
            DateTimeOffset dto = new DateTimeOffset(dt);
            //debitDate.SetDisplayDate(dto);
            
            ledgerTransaction lt = new ledgerTransaction();
            lt.amount = t.amount.ToString();
            lt.particular = t.particular.uniqueName;
            lt.type = t.type;
            getAllTransaction(t.ledgerUniqueName);
            inCaseOfUpdate = t.ledgerUniqueName;
            debitDate.Date = new DateTime(dateInt[0], dateInt[1], dateInt[2]);
            //ledgerTransactionss.Remove(lt);
            //debitAmount.Text = lt.amount;
            //debitParticular.Text = lt.particular;
            //if (lt.type.ToLower().Equals("credit"))
            //{
            //    typeCombo.SelectedIndex = 1;
            //}
            //else
            //    typeCombo.SelectedIndex = 0;
            descriptionBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, string.IsNullOrEmpty(t.ledgerDiscription) ? string.Empty : t.ledgerDiscription);
            tagTextBox.Text = string.IsNullOrEmpty(t.ledgerTag) ? string.Empty : t.ledgerTag;
            voucherTypeCombo.SelectedIndex = setVoucherType(t.ledgerVoucher.shortCode);
            showToastNotification("Please click on edit for that entry.");
        }

        public int setVoucherType(string voucher)
        {
            int i = 0;
            foreach (var v in voucherList)
            {
                if (v.Value.ToLower().Equals(voucher.ToLower()))
                {
                    return i;
                }
                i++;
            }
            return i-1;
        }

        public void getAllTransaction(string ledgerUniquename)
        {
            var ledgerList = al.ledgers.Where(x => x.uniqueName.ToLower().Equals(ledgerUniquename)).ToList()[0];
            clearDetails(false);
            convertTransactionToLedgerTransaction(ledgerList.transactions);
        }

        public void convertTransactionToLedgerTransaction(ObservableCollection<transaction> transactionList)
        {
            foreach (transaction transaction in transactionList)
            {
                ledgerTransaction lt = new ledgerTransaction();
                lt.amount = transaction.amount.ToString();
                lt.particular = transaction.particular.uniqueName;
                lt.type = transaction.type;
                ledgerTransactionss.Add(lt);
            }
        }

        private void debitAmount_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            //TextBox tb = sender as TextBox;
            //if (!Regex.IsMatch(tb.Text, "^\\d*\\.?\\d*$") && tb.Text != "")
            //{
            //    int pos = tb.SelectionStart - 1;
            //    tb.Text = tb.Text.Remove(pos, 1);
            //    tb.SelectionStart = pos;
            //}
        }

        private void debitAmount_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            if (!Regex.IsMatch(sender.Text, "^\\d*\\.?\\d*$") && sender.Text != "")
            {
                int pos = sender.SelectionStart - 1;
                sender.Text = sender.Text.Remove(pos, 1);
                sender.SelectionStart = pos;
            }
        }

        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
