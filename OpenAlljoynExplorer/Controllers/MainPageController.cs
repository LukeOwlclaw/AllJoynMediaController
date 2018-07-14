using System;
using DeviceProviders;
using OpenAlljoynExplorer.Models;
using OpenAlljoynExplorer.Support;

namespace OpenAlljoynExplorer.Controllers
{
    /// <summary>
    /// Detects all available AllJoyn Services.
    /// </summary>
    public class AllJoynController
    {
        private AllJoynModel VM;

        public AllJoynController(AllJoynModel VM)
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

            AllJoynService service = null;
            service = new AllJoynService(args.Service);
            await service.ReadIconAsync();
            service.ReadAll();

            await Dispatcher.Dispatch(() =>
            {
               
                VM.AllJoynServices.Add(service);
                
            });
            
        }
    }
}