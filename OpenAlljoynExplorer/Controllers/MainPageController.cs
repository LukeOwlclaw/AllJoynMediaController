using System;
using System.Linq;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;
using Windows.UI.Xaml.Controls;

namespace OpenAlljoynExplorer.Controllers
{
    /// <summary>
    /// Detects all available AllJoyn Services.
    /// </summary>
    public class AllJoynController
    {
        private AllJoynModel VM;
        private Frame mNavigationFrame;
        private bool favoriteNavigationEnabled;

        public AllJoynController(AllJoynModel VM, Frame frame)
        {
            this.VM = VM;
            mNavigationFrame = frame;
        }

        internal void Start(bool favoriteNavigationEnabled)
        {
            DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
            p.ServiceJoined += ServiceJoined;
            p.ServiceDropped += ServiceDropped;
            p.Start();
            this.favoriteNavigationEnabled = favoriteNavigationEnabled;
        }

        private void ServiceDropped(IProvider sender, ServiceDroppedEventArgs args)
        {
        }


        private async void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
        {
            AllJoynService service = null;
            try
            {
                service = new AllJoynService(args.Service);
                await Favorite.SetAvailableFavorite(VM.Favorites, service);

                MethodModel methodModel = null;
                if (favoriteNavigationEnabled && (methodModel = Favorite.GetFavoriteModel(service)) != null)
                {
                    await Dispatcher.Dispatch(() =>
                    {
                        mNavigationFrame.Navigate(typeof(Pages.MethodPage), methodModel);
                    });
                }
                else
                {
                    await service.ReadIconAsync();
                    service.ReadAll();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }

            await Dispatcher.Dispatch(() =>
            {
                VM.AllJoynServices.Add(service);
            });
        }
    }
}