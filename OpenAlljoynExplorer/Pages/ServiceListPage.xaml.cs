using OpenAlljoynExplorer.Controllers;
using OpenAlljoynExplorer.Models;
using VariableItemListView.Support;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

        public ServiceListPage()
        {
            VM = new AllJoynModel();
            VM.Favorites = new ObservableCollectionThreadSafe<Favorite>();
            VM.Favorites.AddRange(Favorite.GetAll());
            this.InitializeComponent();
            Loaded += ServiceListPage_Loaded;
            this.Unloaded += ServiceListPage_Unloaded;
        }

        private void ServiceListPage_Unloaded(object sender, RoutedEventArgs e)
        {
            Controller.Dispose();
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

            Controller.Start();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
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

        private async void FavoriteList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var fav = e.ClickedItem as Favorite;
            if (fav.IsAvailable == false)
                return;
            var methodModel = new MethodModel { Service = fav.Service, Interface = fav.Interface, Method = fav.Method };
            await Support.Dispatcher.Dispatch(() =>
            {
                Frame.Navigate(typeof(MethodPage), methodModel);
            });
        }

        private void FavoriteList_RightClick(object sender, Windows.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            var fav = ((FrameworkElement)e.OriginalSource).DataContext as Favorite;
            var removed = Favorite.Remove(fav);
            if (removed)
            {
                var d = Support.Dispatcher.Dispatch(() =>
                {
                    VM.Favorites.Remove(fav);
                });
            }
        }
    }
}
