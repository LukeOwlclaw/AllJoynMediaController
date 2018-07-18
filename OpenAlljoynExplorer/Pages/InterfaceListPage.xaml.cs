﻿using DeviceProviders;
using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace OpenAlljoynExplorer.Pages
{
    /// <summary>
    /// Page that shows all interfaces offered by given AllJoyn Object (which is part of a AllJoyn Service)
    /// </summary>
    public sealed partial class InterfaceListPage : BackBasePage
    {
        public IList<IInterface> VM { get; set; }

        public ServicePageController Controller { get; set; }

        public InterfaceListPage() : base()
        {
            this.InitializeComponent();
            Loaded += InterfacePage_Loaded;
        }

        private void InterfacePage_Loaded(object sender, RoutedEventArgs e)
        {
            //Controller = new ServicePageController(VM);
            //Controller.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
            VM = (IList<IInterface>)e.Parameter;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void ListView_IInterfaceClick(object sender, ItemClickEventArgs e)
        {
            IInterface allJoynInterface = e.ClickedItem as IInterface;
            this.Frame.Navigate(typeof(InterfacePage), allJoynInterface);
        }

    }
}