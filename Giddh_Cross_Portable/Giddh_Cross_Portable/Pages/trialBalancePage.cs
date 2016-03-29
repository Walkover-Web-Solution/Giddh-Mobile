using Giddh_Cross_Portable.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Giddh_Cross_Portable.Pages
{
    public class trialBalancePage : ContentPage
    {
        DatePicker toDate = new DatePicker();
        DatePicker fromDate = new DatePicker();
        public trialBalancePage()
        {
            
            List<ContentPage> pages = new List<ContentPage>(0);

            ListView trialBalanceListView = new ListView()
            {
                ItemsSource = Constants.GWTrialBalance,
                RowHeight = 100
            };
            trialBalanceListView.ItemSelected += TrialBalanceListView_ItemSelected;            
            trialBalanceListView.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));



            toDate = new DatePicker();
            fromDate = new DatePicker() {
                //Date = new DateTime().AddDays(-30),
                //MaximumDate = new DateTime()
            };
            var applyButton = new Button()
            {
                Text = "Apply"                
            };
            
            var dateStack = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children = { fromDate, toDate, applyButton }
            };
            CarouselPage cPage = new CarouselPage();
            cPage.Children.Add(createFirstPage());
            cPage.Children.Add(createSecondPage());
            var pageStack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                //Children = { createFirstPage(), createSecondPage() }
            };
            var parentStack = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { dateStack, pageStack }
            };
            Content = parentStack;
            applyButton.Clicked += async (sender, e) =>
            {
                await App.Instance.getTrialBalance(fromDate.Date, toDate.Date);
                parentStack = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = { dateStack, createPage() }
                };
                Content = parentStack;
            };
        }

        private async void ApplyButton_Clicked(object sender, EventArgs e)
        {
            await App.Instance.getTrialBalance(fromDate.Date, toDate.Date);
            //this.Content = createPage();
            //trialBalanceListView.ItemsSource = Constants.GWTrialBalance;
            //Content = parentStack;
        }

        private void TrialBalanceListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }

        private ContentPage createFirstPage()
        {            
            ContentPage newPage = new ContentPage();
            ListView firstGroupList = new ListView()
            {
                RowHeight = 100,
                ItemsSource = Constants.assLiGroup.groupList
            };
            firstGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            Label firstGroupOpeningTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold
            };
            firstGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "openingTotal");
            Label firstGroupClosingTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold
            };
            firstGroupClosingTotalLabel.SetBinding(Label.TextProperty, "closingTotal");
            Label firstGroupPercentageLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };
            firstGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            StackLayout firstGroupLabelStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { firstGroupOpeningTotalLabel, firstGroupPercentageLabel, firstGroupClosingTotalLabel }
            };
            firstGroupLabelStackLayout.BindingContext = Constants.assLiGroup;
            StackLayout firstStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { firstGroupList, firstGroupLabelStackLayout }
            };
            firstStack.BindingContext = Constants.assLiGroup;
            newPage.Content = firstStack;
            return newPage;
        }

        private ContentPage createSecondPage()
        {
            ContentPage newPage = new ContentPage();
            ListView secondGroupList = new ListView()
            {
                RowHeight = 100,
                ItemsSource = Constants.inExpGroup.groupList
            };
            secondGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            Label secondGroupOpeningTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontAttributes = FontAttributes.Bold
            };
            secondGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "openingTotal");
            Label secondGroupClosingTotalLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                FontAttributes = FontAttributes.Bold
            };
            secondGroupClosingTotalLabel.SetBinding(Label.TextProperty, "closingTotal");
            Label secondGroupPercentageLabel = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold
            };
            secondGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            StackLayout secondGroupLabelStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Children = { secondGroupOpeningTotalLabel, secondGroupPercentageLabel, secondGroupClosingTotalLabel }
            };
            secondGroupLabelStackLayout.BindingContext = Constants.inExpGroup;
            StackLayout secondStack = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { secondGroupList, secondGroupLabelStackLayout }
            };
            secondStack.BindingContext = Constants.inExpGroup;
            newPage.Content = secondStack;
            return newPage;
        }

        private StackLayout createPage()
        {
            List<ContentPage> pages = new List<ContentPage>(0);
            pages.Add(createFirstPage());
            pages.Add(createSecondPage());
            return new StackLayout();
            //ListView firstGroupList = new ListView()
            //{
            //    RowHeight = 100,
            //    ItemsSource = Constants.assLiGroup.groupList
            //};
            //firstGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            //ListView secondGroupList = new ListView()
            //{
            //    RowHeight = 100,
            //    ItemsSource = Constants.inExpGroup.groupList
            //};
            //secondGroupList.ItemTemplate = new DataTemplate(typeof(trialBalanceCell));
            ////secondGroupList.SetBinding(ListView.ItemsSourceProperty, "groupList");
            //Label firstGroupOpeningTotalLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    FontAttributes = FontAttributes.Bold
            //};
            //firstGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "openingTotal");
            //Label firstGroupClosingTotalLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.EndAndExpand,
            //    FontAttributes = FontAttributes.Bold
            //};
            //firstGroupClosingTotalLabel.SetBinding(Label.TextProperty, "closingTotal");
            //Label firstGroupPercentageLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.Center,
            //    FontAttributes = FontAttributes.Bold
            //};
            //firstGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            //Label secondGroupOpeningTotalLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    FontAttributes = FontAttributes.Bold
            //};
            //secondGroupOpeningTotalLabel.SetBinding(Label.TextProperty, "openingTotal");
            //Label secondGroupClosingTotalLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.EndAndExpand,
            //    FontAttributes = FontAttributes.Bold
            //};
            //secondGroupClosingTotalLabel.SetBinding(Label.TextProperty, "closingTotal");
            //Label secondGroupPercentageLabel = new Label()
            //{
            //    HorizontalOptions = LayoutOptions.Center,
            //    FontAttributes = FontAttributes.Bold
            //};
            //secondGroupPercentageLabel.SetBinding(Label.TextProperty, "percentage");
            ////DatePicker fromDatePicker = new DatePicker();
            ////DatePicker toDatePicker = new DatePicker();
            ////Button applyButton = new Button()
            ////{
            ////    Text = "Apply"
            ////};
            ////applyButton.Clicked += ApplyButton_Clicked;
            ////StackLayout datePickerStackLayout = new StackLayout()
            ////{
            ////    Orientation = StackOrientation.Horizontal,
            ////    HorizontalOptions = LayoutOptions.FillAndExpand,
            ////    Children = { fromDatePicker, toDatePicker, applyButton }
            ////};
            //StackLayout firstGroupLabelStackLayout = new StackLayout()
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Children = { firstGroupOpeningTotalLabel, firstGroupPercentageLabel, firstGroupClosingTotalLabel }
            //};
            //firstGroupLabelStackLayout.BindingContext = Constants.assLiGroup;
            //StackLayout firstStack = new StackLayout()
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Children = { firstGroupList, firstGroupLabelStackLayout }
            //};
            //firstStack.BindingContext = Constants.assLiGroup;
            ////firstStack.SetBinding(StackLayout.BindingContextProperty, Constants.assLiGroup);
            //StackLayout secondGroupLabelStackLayout = new StackLayout()
            //{
            //    Orientation = StackOrientation.Horizontal,
            //    Children = { secondGroupOpeningTotalLabel, secondGroupPercentageLabel, secondGroupClosingTotalLabel }
            //};
            //secondGroupLabelStackLayout.BindingContext = Constants.inExpGroup;
            //StackLayout secondStack = new StackLayout()
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Children = { secondGroupList, secondGroupLabelStackLayout }
            //};
            //secondStack.BindingContext = Constants.inExpGroup;
            //var mainStackLayout = new StackLayout()
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Children = { firstStack, secondStack }
            //};
            //return mainStackLayout;
        }
    }
}
