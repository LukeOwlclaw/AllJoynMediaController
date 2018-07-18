using DeviceProviders;
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
    /// Page that shows a given AllJoyn Interface (including Properties, Methods, Signals)
    /// (Interface is part of Object which is part of AllJosn Service)
    /// </summary>
    public sealed partial class InterfacePage : BackBasePage
    {
        public IInterface VM { get; set; }

        public ServicePageController Controller { get; set; }

        public InterfacePage() : base()
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
            VM = (IInterface)e.Parameter;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void ListView_MethodClick(object sender, ItemClickEventArgs e)
        {
            var method = e.ClickedItem as IMethod;
            this.Frame.Navigate(typeof(MethodPage), method);
        }

        private void ListView_SignalClick(object sender, ItemClickEventArgs e)
        {
            var signal = e.ClickedItem as ISignal;
        }

        private void ListView_PropertyClick(object sender, ItemClickEventArgs e)
        {
            var property = e.ClickedItem as IProperty;
        }
    }
}
