using System;
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
        private readonly Frame mNavigationFrame;

        public AllJoynController(AllJoynModel VM, Frame frame)
        {
            this.VM = VM;
            mNavigationFrame = frame;
        }

        internal void Start()
        {
            DeviceProviders.AllJoynProvider p = new DeviceProviders.AllJoynProvider();
            p.ServiceJoined += ServiceJoined;
            p.ServiceDropped += ServiceDropped;
            p.Start();
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

                await service.ReadIconAsync();
                //service.ReadAll();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }

            await Dispatcher.Dispatch(() =>
            {
                try
                {
                    VM.AllJoynServices.Add(service);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
            });
        }
    }
}