using System;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;

namespace OpenAlljoynExplorer.Controllers
{
    public class MainPageController
    {
        private MainPageModel VM;

        public MainPageController(MainPageModel VM)
        {
            this.VM = VM;
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
            //throw new NotImplementedException();
        }

        private async void ServiceJoined(IProvider sender, ServiceJoinedEventArgs args)
        {
            var service = new AllJoynService(args.Service);

            Dispatcher.Dispatch(() =>
            {
                VM.AllJoynServices.Add(service);
            });

            await service.ReadIconAsync();
            

        }
    }
}