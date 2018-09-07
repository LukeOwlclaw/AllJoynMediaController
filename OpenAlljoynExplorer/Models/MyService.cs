using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.Models
{
    public class MyService : IService
    {
        //private MyAboutData mAboutData;
        //private List<MyBusObject> mBusObject = new List<MyBusObject>();
        private  MyProvider mProvider;
        //private MyService mService;

        public MyService(IService service)
        {
            AboutData = new MyAboutData(service.AboutData);
            Objects = new List<IBusObject>();
            foreach (var busObject in service.Objects)
            {
                Objects.Add(new MyBusObject(busObject));
            }
            //mProvider = new MyProvider(service.Provider);
            //this.service = service;
        }

        public AllJoynStatus Ping()
        {
            throw new NotImplementedException();
        }

        public AllJoynSession JoinSession(ushort port)
        {
            throw new NotImplementedException();
        }

        public AllJoynSession JoinSession()
        {
            throw new NotImplementedException();
        }

        public bool ImplementsInterface(string interfaceName)
        {
            throw new NotImplementedException();
        }

        public IBusObject GetBusObject(string path)
        {
            throw new NotImplementedException();
        }

        public IList<IBusObject> GetBusObjectsWhichImplementInterface(string interfaceName)
        {
            throw new NotImplementedException();
        }

        public ushort PreferredPort { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ushort AnnouncedPort => throw new NotImplementedException();

        public IProvider Provider => throw new NotImplementedException();

        public IAboutData AboutData { get; }

        public string Name => throw new NotImplementedException();

        public IList<IBusObject> Objects { get; }
    }
}
