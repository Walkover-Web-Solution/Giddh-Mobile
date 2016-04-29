﻿using GiddhDesktop.Common;
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

namespace GiddhDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class companyPage : Page
    {
        public companyPage()
        {
            this.InitializeComponent();
            getCompanies();
        }

        public async void getCompanies()
        {
            await server.companies();
            setCompanies();
        }

        public void setCompanies()
        {
            this.companyComboBox.ItemsSource = Constants.companies;
            if (Constants.companies.Count > 1)
                this.companyComboBox.SelectedIndex = 0;
        }
    }
    
}
