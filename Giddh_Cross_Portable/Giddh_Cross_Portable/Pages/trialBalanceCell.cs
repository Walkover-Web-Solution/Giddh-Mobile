using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    class trialBalanceCell : ViewCell
    {
        public trialBalanceCell()
        {
            var nameLayout = CreateNameLayout();
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 18,
                TextColor = Color.Gray,
                //BackgroundColor = Color.FromRgb(100, 100, 100),
                HorizontalTextAlignment = TextAlignment.Start,
                //VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 30                
            };            
            nameLabel.SetBinding(Label.TextProperty, "name");
            //nameLabel.BindingContextChanged += NameLabel_BindingContextChanged1;
            var topBorder = new BoxView()
            {
                HeightRequest = 2,
                BackgroundColor = Color.Gray
            };
            var bottomBorder = new BoxView()
            {
                HeightRequest = 2,
                BackgroundColor = Color.Gray
            };
            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel, bottomBorder, nameLayout }
            };
            View = viewLayout;
        }

        private void NameLabel_BindingContextChanged1(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            lb.Text = "  " + lb.Text;
        }

        private void makeInProper(string str)
        {
            //TextInfo ti = new CultureInfo
        }

        static StackLayout CreateNameLayout()
        {
            var obTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "O/b",
                TextColor = Color.Gray,
                FontSize = 12
            };
            var cbTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                XAlign = TextAlignment.End,                
                Text = "C/b",
                TextColor = Color.Gray,
                FontSize = 12
            };
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //TextColor = Color.Black
            };
            nameLabel.SetBinding(Label.TextProperty, "creditTotal");
            nameLabel.BindingContextChanged += TwitterLabel_BindingContextChanged;

            var twitterLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                //TextColor = Color.Black
            };
            twitterLabel.SetBinding(Label.TextProperty, "debitTotal");
            twitterLabel.BindingContextChanged += TwitterLabel_BindingContextChanged;

            var obLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //TextColor = Color.Black
            };
            obLabel.SetBinding(Label.TextProperty, "oBalance");

            var cbLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                //TextColor = Color.Black
            };
            cbLabel.SetBinding(Label.TextProperty, "cBalance");

            var nameLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { nameLabel, twitterLabel }
            };

            var balance = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { obLabel}}, new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { cbLabel}} }
            };

            var sendThis = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { balance, nameLayout }
            };
            return sendThis;
        }        

        private static void TwitterLabel_BindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                Label lb = sender as Label;
                if (lb.Text.StartsWith("d"))
                {
                    lb.Text = lb.Text.Remove(0, 1);
                    //lb.Text = lb.Text+ "Cr." ;//"↓" + lb.Text;
                }
                if (lb.Text.StartsWith("u"))
                {
                    lb.Text = lb.Text.Remove(0, 1);
                    //lb.Text = "Dr." + lb.Text;//"↑" + lb.Text;
                }
            }
            catch (Exception ex)
            { }
        }

        private static void NameLabel_BindingContextChanged(object sender, EventArgs e)
        {
            try
            {
                Label lb = sender as Label;
                if (lb.Text.StartsWith("d"))
                {
                    lb.Text = lb.Text.Remove(0, 1);
                    lb.Text = lb.Text + "↓";
                }
                if (lb.Text.StartsWith("u"))
                {
                    lb.Text = lb.Text.Remove(0, 1);
                    lb.Text = lb.Text + "↑";
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
