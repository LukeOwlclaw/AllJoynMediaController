using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.Models
{
    class DummyNavigationService : IService
    {
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

        public ushort PreferredPort { get => 0; set => throw new NotImplementedException(); }

        public ushort AnnouncedPort => 0;

        public IProvider Provider => null;

        public IAboutData AboutData => new DummyNavigationAboutData();

        public string Name => "test";

        public IList<IBusObject> Objects => null;
    }
}
