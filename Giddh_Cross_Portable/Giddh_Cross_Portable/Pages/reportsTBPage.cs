using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    class reportsTBPage : ContentPage
    {
        public reportsTBPage()
        {
            Title = "Reports";
            ListView firstGroupList = new ListView()
            {
                RowHeight = 120,
                ItemsSource = Constants.GWTrialBalance,
                VerticalOptions = LayoutOptions.Start,
                IsEnabled = true
            };
            firstGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            //StackLayout firstStack = new StackLayout()
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Padding = new Thickness(5, 5, 5, 0),
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            //firstStack.Children.Add(act);

            var obTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "opening balance",
                TextColor = Color.Gray,
                FontSize = 14
            };
            var cbTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                XAlign = TextAlignment.End,
                Text = "closing balance",
                TextColor = Color.Gray,
                FontSize = 14
            };
            var cTotalTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = "credit total",
                TextColor = Color.Gray,
                FontSize = 14
            };
            var dTotalTextLabel = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                XAlign = TextAlignment.End,
                Text = "debit total",
                TextColor = Color.Gray,
                FontSize = 14
            };
            StackLayout headerStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { obTextLabel, cbTextLabel } }, new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { cTotalTextLabel, dTotalTextLabel } } }
            };
            //firstStack.Children.Add(headerStack);
            //firstStack.Children.Add(firstGroupList);
            //if (reset)
            {
                //firstStack.Children.Add(dateStack);
            }
            var stack = new StackLayout
            {
                Children = { headerStack, firstGroupList },
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Content = stack;
        }
    }
}
