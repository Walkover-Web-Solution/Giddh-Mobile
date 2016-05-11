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
        public accountLedgerPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var account = (accountDetail)e.Parameter;
            getLedger(account);            
        }

        public async void getLedger(accountDetail acDetail)
        {
            accountLedger al = await server.getAccountLedgers(acDetail);
            mainGrid.DataContext = al;
        }
    }
}
