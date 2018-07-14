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
    /// Page that shown all objects belonging to given AllJoyn service.
    /// </summary>
    public sealed partial class ObjectListPage : BackBasePage
    {
        public IService VM { get; set; }

        public ServicePageController Controller { get; set; }

        public ObjectListPage() : base()
        {
            this.InitializeComponent();
            Loaded += ServicePage_Loaded;
        }

        private void ServicePage_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new ServicePageController(VM);
            Controller.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            BackButton.IsEnabled = this.Frame.CanGoBack;
            VM = (IService)e.Parameter;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

      

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var paths = VM.Objects.Select(o => o.Path).ToList();
            var interfaces = VM.Objects.Select(o => o.Interfaces).ToList();
            //interfaces..FirstOrDefault().na
            var path= VM.Objects.Select(o => o.Path).ToList();
        }

        private void ListView_IBusObjectClick(object sender, ItemClickEventArgs e)
        {
            IBusObject busObject = e.ClickedItem as IBusObject;
            this.Frame.Navigate(typeof(InterfaceListPage), busObject.Interfaces);
        }

    }
}
