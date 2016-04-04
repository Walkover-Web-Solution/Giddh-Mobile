using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class trialBalanceTabbedPage : TabbedPage
    {
        ActivityIndicator act = new ActivityIndicator();
        ActivityIndicator Ract = new ActivityIndicator();

        ListView firstGroupList = new ListView();
        ListView secondGroupList = new ListView();

        DatePicker toDate = new DatePicker();
        DatePicker fromDate = new DatePicker();
        Button applyButton = new Button();
        StackLayout dateStack = new StackLayout();
        StackLayout dateStackSecond = new StackLayout();

        accountListPage acntPage = new accountListPage();

        public trialBalanceTabbedPage()
        {
            try
            {                
                //this.BackgroundColor = Color.White; 
                createDateStack();
                this.Children.Add(createReportsView());
                this.Children.Add(createFirstPage(true));
                this.Children.Add(acntPage);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnAppearing()
        {
            //this.BackgroundColor = Color.White;
            base.OnAppearing();
            act.IsRunning = true;
            act.HeightRequest = 0;
            Ract.IsRunning = true;
            Ract.HeightRequest = 0;
        }

        private StackLayout createDateStack(bool first = true)
        {
            DateTime dt = DateTime.Now;
            if (dt.Date < new DateTime(dt.Year, 4, 1))
            {
                //dt = new DateTime(dt.AddYears(-1).Year, 4, 1);
                dt = dt.AddDays(-30);
            }            
            fromDate = new DatePicker()
            {
                //MaximumDate = DateTime.Now,
                //MinimumDate = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1),
                //BackgroundColor = Color.Black
            };
            toDate = new DatePicker()
            {
                //MaximumDate = DateTime.Now,
                //MinimumDate = fromDate.Date,
                //BackgroundColor = Color.Black
            };
            fromDate.DateSelected += FromDate_DateSelected;
            toDate.DateSelected += ToDate_DateSelected1;
            fromDate.Date = dt;

            Title = "";

            applyButton = new Button()
            {
                Text = "Apply",
                //BackgroundColor = Color.Black
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
                VerticalOptions = LayoutOptions.EndAndExpand
                //Children = { fromDate, toDate, applyButton }
            };
            if (first)
            { return dateStack; }
            else
                return dateStackSecond;
        }

        private void ToDate_DateSelected1(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate > DateTime.Now)
            {
                (sender as DatePicker).Date = DateTime.Now;
            }
            else if (e.NewDate < fromDate.Date)
            {
                (sender as DatePicker).Date = DateTime.Now;
            }
        }

        private void ToDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            
        }

        private void FromDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate > DateTime.Now)
            {
                (sender as DatePicker).Date = DateTime.Now;
            }
            if (e.NewDate > toDate.Date)
            {
                toDate.Date = DateTime.Now;
            }
        }

        private async void ApplyButton_Clicked(object sender, EventArgs e)
        {
            //firstGroupList.IsRefreshing = true;            
            act.HeightRequest = 40;
            await App.Instance.getTrialBalance(fromDate.Date, toDate.Date);
            act.HeightRequest = 0;
            //firstGroupList.IsRefreshing = false;
            this.Children.RemoveAt(0);
            this.Children.RemoveAt(0);
            this.Children.RemoveAt(0);
            this.Children.Add(createReportsView());
            this.Children.Add(createFirstPage());
            this.Children.Add(acntPage);
            this.CurrentPage = this.Children[1];
        }

        private ContentPage createFirstPage(bool reset = false)
        {
            ContentPage newView = new ContentPage();
            try
            {
                
                newView.Title = "trialbalance";
                newView.Style = new Style(typeof(Label));
                firstGroupList = new ListView()
                {
                    RowHeight = 120,
                    ItemsSource = Constants.GWTrialBalance,
                    VerticalOptions = LayoutOptions.Start,
                    IsEnabled = true
                };
                firstGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
                StackLayout firstStack = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical,
                    Padding = new Thickness(5, 5, 5, 0),
                    VerticalOptions = LayoutOptions.FillAndExpand        
                };
                firstStack.Children.Add(act);
                
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
                    Children = { new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { obTextLabel,cbTextLabel} }, new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { cTotalTextLabel, dTotalTextLabel } } }
                };                
                firstStack.Children.Add(headerStack);
                firstStack.Children.Add(firstGroupList);
                //if (reset)
                {
                    firstStack.Children.Add(dateStack);
                }
                newView.Content = new StackLayout
                {
                    Children = { act, headerStack, firstGroupList, dateStack },
                    VerticalOptions = LayoutOptions.FillAndExpand
                };                
            }
            catch (Exception ex)
            { }
            return newView;
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

        private ContentPage createReportsView()
        {
            Ract.HeightRequest = 0;
            ContentPage reportView = new ContentPage();
            reportView.Title = "reports";
            ListView details = new ListView()
            {
                IsEnabled = false,
                HasUnevenRows = true
            };
            details.ItemTemplate = new DataTemplate(typeof(reportsCell));
            details.ItemsSource = Constants.reportsList;
            Button refreshButton = new Button()
            {
                Text = "Refresh",
                BorderWidth = 0,
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            refreshButton.Clicked += RefreshButton_Clicked;
            reportView.Content = new StackLayout() { Children = { Ract, details, refreshButton }, Orientation = StackOrientation.Vertical };
            return reportView;
        }

        private async void RefreshButton_Clicked(object sender, EventArgs e)
        {
            Ract.HeightRequest = 40;
            await App.Instance.getTrialBalance(fromDate.Date, toDate.Date);
            Ract.HeightRequest = 0;
            this.Children.RemoveAt(0);
            this.Children.RemoveAt(0);
            this.Children.RemoveAt(0);
            this.Children.Add(createReportsView());
            this.Children.Add(createFirstPage());
            this.Children.Add(acntPage);
        }

        private ContentPage createSecondView()
        {
            ContentPage newView = new ContentPage();
            newView.Title = "Profit & Loss";
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
                Children = { secondGroupList, secondGroupLabelStackLayout, createDateStack(false) }
            };
            secondStack.BindingContext = Constants.inExpGroup;
            newView.Content = secondStack;
            return newView;
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

        protected override bool OnBackButtonPressed()
        {
            App.Instance.MainPage = new NavigationPage(new ProfilePage());
            //App.Instance._NavPage.Navigation.PopAsync();
            return true;
            //return base.OnBackButtonPressed();
        }
    }
}
