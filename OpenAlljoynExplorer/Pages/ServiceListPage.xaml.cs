using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VariableItemListView.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OpenAlljoynExplorer.Pages
{
    /// <summary>
    /// Page that lists of available AllJoyn Services (MainPage)
    /// </summary>
    public sealed partial class ServiceListPage : Page
    {
        public AllJoynModel VM { get; set; }
        public AllJoynController Controller { get; set; }
        public bool DisableFavoriteNavigation { get; set; }

        public ServiceListPage()
        {
            VM = new AllJoynModel();
            this.InitializeComponent();
            Loaded += ServiceListPage_Loaded;
        }

        private void ServiceListPage_Loaded(object sender, RoutedEventArgs e)
        {
            Controller = new AllJoynController(VM, this.Frame);
            //// for testing, go directly to this AllJoyn interface:
            //Controller.GoTo(frame: this.Frame, deviceId: "2e1b9ed0-8429-4934-a566-9c44cda28b99", objectPath: "/About",
            //    interfaceName: "org.alljoyn.About", methodName: "GetAboutData");
            ////interfaceName: "org.alljoyn.About", methodName: "GetObjectDescription");

            //// AllPlay MediaPlayer
            //Controller.GoTo(frame: this.Frame, deviceId: "Willy", objectPath: "/net/allplay/MediaPlayer",
            //    interfaceName: "net.allplay.MediaPlayer", methodName: "GetPlaylist");
            //Controller.GoTo(frame: this.Frame, deviceId: "Aploris", objectPath: "/net/allplay/MediaPlayer",
            //    interfaceName: "net.allplay.MediaPlayer", methodName: "GetPlaylist");


            Controller.Start(!DisableFavoriteNavigation);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
                DisableFavoriteNavigation = true;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            AllJoynService allJoynService = e.ClickedItem as AllJoynService;
            this.Frame.Navigate(typeof(ObjectListPage), allJoynService);

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(this.GetType());
        }
    }
}
