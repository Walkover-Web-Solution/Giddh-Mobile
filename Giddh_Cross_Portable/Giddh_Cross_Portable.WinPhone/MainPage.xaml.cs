using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Giddh_Cross_Portable.WinPhone
{
    public partial class MainPage : global::Xamarin.Forms.Platform.WinPhone.FormsApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            SupportedOrientations = SupportedPageOrientation.Portrait;

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(Giddh_Cross_Portable.App.Instance);
            Giddh_Cross_Portable.App.Instance.MainPage = Giddh_Cross_Portable.App.Instance.GetMainPage();
        }
    }
}
