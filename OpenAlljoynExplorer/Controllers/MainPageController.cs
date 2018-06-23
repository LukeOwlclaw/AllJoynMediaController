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

            AllJoynService service = null;
            await Dispatcher.Dispatch(() =>
            {
                // This is bad: We are doing everything on the UI thread.
                // And somehow asynchronously/in random order:
                // Try adding to AlljoynService.PropMap: {"Services", typeof(IList<IService>) }
                // Then order of properties will surely be messed up. It may even be now.
                // The problem however is, that the created VariableListViewModel (which is created correctly) is rendered incorrectly. E.g. we have seen a duplicated entry and entries in wrong order.
                service = new AllJoynService(args.Service);
                VM.AllJoynServices.Add(service);
                var runAsync = service.ReadIconAsync();
                runAsync = service.ReadAllAsync();
            });
            
        }
    }
}