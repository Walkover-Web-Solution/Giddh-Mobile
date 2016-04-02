//using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class accountLedgerCell : ViewCell
    {
        public accountLedgerCell()
        {
            var ledgerList = new ListView
            {
                IsEnabled = false,
                HasUnevenRows = true,
                RowHeight = -1
            };
            //ledgerList.SetBinding(ListView.HeaderProperty, "Date");
            ledgerList.SetBinding(ListView.ItemsSourceProperty, "transactions");
            ledgerList.ItemTemplate = new DataTemplate(typeof(aLedgerInsideCell));
            View = new StackLayout {
                Children = { ledgerList }
            };
        }
    }

    public class aLedgerInsideCell : ViewCell
    {
        public aLedgerInsideCell()
        {
            var letsC = this;
            var nameLabel = new Label
            {
                FontSize = 18,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                WidthRequest = 160                
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
            var debit = new Label
            {
                FontSize = 18,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                MinimumWidthRequest = 80,
                HorizontalTextAlignment = TextAlignment.End
            };
            debit.SetBinding(Label.TextProperty, "debitAmount");
            debit.BindingContextChanged += Debit_BindingContextChanged;
            var credit = new Label
            {
                FontSize = 18,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                MinimumWidthRequest = 80,
                HorizontalTextAlignment = TextAlignment.End
            };
            credit.SetBinding(Label.TextProperty, "creditAmount");
            credit.BindingContextChanged += Debit_BindingContextChanged;
            View = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { nameLabel, credit, debit },
                Padding = new Thickness(0,5)
            };
        }

        private void Debit_BindingContextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            lb.Text = (Convert.ToDouble(lb.Text)).ToString("N").Replace(".00", "");
            if (lb.Text.Equals("0"))
                lb.Text = "";
        }
    }

    public class ledgerGroupHeaderCell : ViewCell
    {
        public ledgerGroupHeaderCell()
        {
            var dateLabel = new Label
            {
                FontSize = 28,
                TextColor = Color.Gray
            };
            dateLabel.SetBinding(Label.TextProperty, "Date");
            View = new StackLayout
            {
                Children = { dateLabel },
                Padding = new Thickness(0,10)
            };
        }
    }

    public class transactionCell : ViewCell
    {
        public transactionCell()
        {
            var nameLabel = new Label
            {
                FontSize = 18
            };
            nameLabel.SetBinding(Label.TextProperty, "particular.name");
            var uniqueLabel = new Label
            {
                FontSize = 18
            };
            uniqueLabel.SetBinding(Label.TextProperty, "amount");
            var stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel, uniqueLabel }
            };
            View = stack;
            
        }
    }
}
