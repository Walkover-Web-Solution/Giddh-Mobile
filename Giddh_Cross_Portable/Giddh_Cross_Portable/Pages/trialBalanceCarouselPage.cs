using Giddh_Cross_Portable.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class trialBalanceCarouselPage : CarouselPage
    {
        ActivityIndicator act = new ActivityIndicator();
        DatePicker toDate = new DatePicker();
        DatePicker fromDate = new DatePicker();
        Button applyButton = new Button();
        StackLayout dateStack = new StackLayout();
        StackLayout dateStackSecond = new StackLayout();
        ListView firstGroupList = new ListView();
        ListView secondGroupList = new ListView();
        public trialBalanceCarouselPage()
        {
            DateTime dt = DateTime.Now;
            if (dt.Date < new DateTime(dt.Year, 4, 1))
            {
                dt = new DateTime(dt.AddYears(-1).Year, 4, 1);
            }
            fromDate = new DatePicker()
            {
                MaximumDate = DateTime.Now,
                MinimumDate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1)
            };
            toDate = new DatePicker()
            {
                MaximumDate = DateTime.Now,
                MinimumDate = fromDate.Date
            };
            fromDate.Date = dt;
            
            Title = "";            
            
            applyButton = new Button()
            {
                Text = "Apply"
            };

            applyButton.Clicked += ApplyButton_Clicked;

            dateStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Children = { fromDate, toDate, applyButton }
            };
            dateStackSecond = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Children = { fromDate, toDate, applyButton }
            };
            try
            {
                Children.Add(createFirstPage(dateStack));
                Children.Add(createSecondPage(dateStackSecond));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async void ApplyButton_Clicked(object sender, EventArgs e)
        {
            firstGroupList.IsRefreshing = true;
            secondGroupList.IsRefreshing = true;
            await App.Instance.getTrialBalance(fromDate.Date, toDate.Date);
            firstGroupList.IsRefreshing = false;
            secondGroupList.IsRefreshing = false;
            this.Children.RemoveAt(0);
            this.Children.RemoveAt(0);
            Children.Add(createFirstPage(dateStack));
            Children.Add(createSecondPage(dateStack));
        }

        private ContentPage createFirstPage(StackLayout datestack)
        {
            ContentPage newPage = new ContentPage();
            newPage.Title = "Trial Balance";
            firstGroupList = new ListView()
            {
                RowHeight = 150,
                ItemsSource = Constants.assLiGroup.groupList,
                VerticalOptions = LayoutOptions.Start,
                IsEnabled = false
            };
            firstGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            Label firstGroupOpeningTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            firstGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "oTotal");
            Label firstGroupClosingTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            firstGroupClosingTotalLabel.SetBinding(Label.TextProperty, "cTotal");
            Label firstGroupPercentageLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            firstGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            firstGroupPercentageLabel.BindingContextChanged += FirstGroupPercentageLabel_BindingContextChanged;
            Label equalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                Text = "=",
                YAlign = TextAlignment.Center,
                TextColor = Color.Gray
            };

            Label signLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                YAlign = TextAlignment.Center,
                TextColor = Color.Gray
            };
            signLabel.SetBinding(Label.TextProperty, "percentage");
            signLabel.BindingContextChanged += SignLabel_BindingContextChanged;
            StackLayout firstGroupLabelStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { firstGroupOpeningTotalLabel, signLabel, firstGroupPercentageLabel, equalLabel, firstGroupClosingTotalLabel }
            };
            firstGroupLabelStackLayout.BindingContext = Constants.assLiGroup;
            StackLayout firstStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5, 5, 5, 0),
                Children = { firstGroupList, firstGroupLabelStackLayout, datestack }
            };

            firstStack.BindingContext = Constants.assLiGroup;
            newPage.Content = firstStack;
            return newPage;
        }

        private void FirstGroupPercentageLabel_BindingContextChanged(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            if (lb.Text.StartsWith("-"))
            {
                lb.Text = lb.Text.Replace("-", "");
            }
            lb.Text = lb.Text + "%";
        }

        private ContentPage createSecondPage(StackLayout datestack)
        {
            ContentPage newPage = new ContentPage();
            newPage.Title = "Profit Loss";
            secondGroupList = new ListView()
            {
                RowHeight = 150,
                ItemsSource = Constants.inExpGroup.groupList,
                VerticalOptions = LayoutOptions.Start,
                IsEnabled = false
            };
            secondGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            Label secondGroupOpeningTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "oTotal");
            Label secondGroupClosingTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupClosingTotalLabel.SetBinding(Label.TextProperty, "cTotal");
            Label secondGroupPercentageLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Gray
            };
            secondGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            secondGroupPercentageLabel.BindingContextChanged += SecondGroupPercentageLabel_BindingContextChanged;

            Label equalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                Text = "=",
                YAlign = TextAlignment.Center,
                TextColor = Color.Gray
            };

            Label signLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                FontAttributes = FontAttributes.Bold,
                YAlign = TextAlignment.Center,
                TextColor = Color.Gray
            };
            signLabel.SetBinding(Label.TextProperty, "percentage");
            signLabel.BindingContextChanged += SignLabel_BindingContextChanged;
            StackLayout secondGroupLabelStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { secondGroupOpeningTotalLabel, signLabel, secondGroupPercentageLabel, equalLabel, secondGroupClosingTotalLabel }
            };
            secondGroupLabelStackLayout.BindingContext = Constants.inExpGroup;
            StackLayout secondStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(5, 5, 5, 0),
                Children = { secondGroupList, secondGroupLabelStackLayout, datestack }
            };
            secondStack.BindingContext = Constants.inExpGroup;
            newPage.Content = secondStack;
            return newPage;
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
                lb.Text = lb.Text.Remove(0, 1);
            }
            lb.Text = lb.Text + "%";
        }

        protected override bool OnBackButtonPressed()
        {
            //App.Instance.goToProfilePage.Invoke();
            //App.Instance._NavPage.Navigation.PopAsync();
            //var navStack = Navigation.NavigationStack;
            //return true;
            return base.OnBackButtonPressed();
        }
    }
}
