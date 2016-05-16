using GiddhDesktop.Common;
using GiddhDesktop.Common.Modal;
using GiddhDesktop.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
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

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            debitDate.Date = new DateTimeOffset(DateTime.Now);
            debitParticular.Text = "";
            debitAmount.Text = "0";
            voucherTypeCombo.SelectedIndex = 0;
            tagTextBox.Text = "";
            descriptionBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
            debitDate.Focus(FocusState.Keyboard);
        }

        private async void createEntryButton_Click(object sender, RoutedEventArgs e)
        {
            ledgerToSend lts = new ledgerToSend();
            ledgerTransaction lt = new ledgerTransaction();
            lt.particular = ((accountDetail)debitParticular.DataContext).uniqueName;
            lt.amount = debitAmount.Text;
            lt.type = typeCombo.SelectionBoxItem.ToString();
            lts.transactions = new ledgerTransaction[] { lt };
            lts.entryDate = debitDate.Date.Value.ToString("dd-MM-yyyy");
            string description = "";
            descriptionBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out description);
            lts.description = description;
            lts.voucherType = (voucherList.Where(x => x.Key.Equals(voucherTypeCombo.SelectionBoxItem.ToString())).ToList())[0].Value.ToString();
            lts.tag = tagTextBox.Text;
            Response response = await server.createNewEntry(lts);
            if (response.status.ToLower().Equals("success"))
            {                
                getLedger(acDetail);
                clearButton_Click(sender, e);
            }
        }

        private void debitParticular_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSuggestBox asb = sender as AutoSuggestBox;
            if (string.IsNullOrEmpty(asb.Text))
            {
                typeCombo.Focus(FocusState.Keyboard);
                //creditDate.Focus(FocusState.Keyboard);
            }
        }

        private void debitAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (!string.IsNullOrEmpty(tb.Text) && !tb.Text.Equals("0"))
            {
                voucherTypeCombo.Focus(FocusState.Keyboard);
            }
        }

        private void creditParticular_LostFocus(object sender, RoutedEventArgs e)
        {
            AutoSuggestBox asb = sender as AutoSuggestBox;
            if (string.IsNullOrEmpty(asb.Text))
            {
                debitDate.Focus(FocusState.Keyboard);
            }
        }

        private async void sendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            string emails = string.Empty;
            emailBox.Document.GetText(Windows.UI.Text.TextGetOptions.UseObjectText, out emails);
            if (string.IsNullOrEmpty(emails))
            {
                var dialog = new MessageDialog("Enter atleaset one email address.", "Information");
                dialog.ShowAsync();
                emailBox.Focus(FocusState.Keyboard);
                return;
            }
            var emailArray = emails.Split(',');
            Response response = await server.mailLedger(emailArray.ToArray(), fromDatePicker.Date.Value.ToString("dd-MM-yyyy"), toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
            //if (response.status.ToLower().Equals("success"))
            {
                emailBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, "");
                var dialog = new MessageDialog(response.message, response.status);
                dialog.ShowAsync();
            }
        }

        private async void downloadButton_Click(object sender, RoutedEventArgs e)
        {
            Response response = await server.exportLedger(fromDatePicker.Date.Value.ToString("dd-MM-yyyy"), toDatePicker.Date.Value.ToString("dd-MM-yyyy"));
            string filePath = response.body["filePath"].ToString();
            Download(filePath);
        }
        public async void Download(string urlToDownload)
        {

            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Downloads;
            folderPicker.ViewMode = PickerViewMode.Thumbnail;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageFile file = await folder.CreateFileAsync(acDetail.name+"ledger.xls", CreationCollisionOption.GenerateUniqueName);
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
    }
}
