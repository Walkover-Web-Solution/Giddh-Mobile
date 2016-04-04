using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class reportsCell : ViewCell
    {
        public reportsCell()
        {
            StackLayout netWorthStack = new StackLayout();
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = 18,
                TextColor = Color.Gray,
                //BackgroundColor = Color.FromRgb(100, 100, 100),
                HorizontalTextAlignment = TextAlignment.Start
            };
            nameLabel.SetBinding(Label.TextProperty, "name");
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
            var secondGroupOpeningTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "oTotal");
            var secondGroupClosingTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupClosingTotalLabel.SetBinding(Label.TextProperty, "cTotal");
            var secondGroupPercentageLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            secondGroupPercentageLabel.BindingContextChanged += SecondGroupPercentageLabel_BindingContextChanged;

            Label equalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                Text = "=",
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Gray
            };

            Label signLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.Gray
            };
            signLabel.SetBinding(Label.TextProperty, "percentage");
            signLabel.BindingContextChanged += SignLabel_BindingContextChanged;
            StackLayout secondGroupLabelStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { secondGroupOpeningTotalLabel, signLabel, secondGroupPercentageLabel, equalLabel, secondGroupClosingTotalLabel },
                Padding = new Thickness(0,10)
            };
            //secondGroupLabelStackLayout.BindingContext = Constants.assLiGroup;
            //netWorthStack.Children.Add(topBorder);
            netWorthStack.Children.Add(nameLabel);
            netWorthStack.Children.Add(bottomBorder);
            netWorthStack.Children.Add(secondGroupLabelStackLayout);
            View = netWorthStack;
        }

        private void SignLabel_BindingContextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb.Text.StartsWith("-"))
            {
                lb.Text = "-";
            }
            else
                lb.Text = "+";
        }

        private void SecondGroupPercentageLabel_BindingContextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb.Text.StartsWith("-"))
            {
                lb.Text = lb.Text.Replace("-", "");
            }
            lb.Text = lb.Text + "%";
        }
    }
}
