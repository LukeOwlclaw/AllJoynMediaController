using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Windows.Foundation;

namespace OpenAlljoynExplorer.Models
{
    class MyProvider: IProvider
    {
        public AllJoynStatus Start()
        {
            throw new NotImplementedException();
        }

        public void Shutdown()
        {
            throw new NotImplementedException();
        }

        public IList<IService> GetServicesWhichImplementInterface(string interfaceName)
        {
            throw new NotImplementedException();
        }

        public IList<IService> Services => throw new NotImplementedException();

        public event TypedEventHandler<IProvider, ServiceJoinedEventArgs> ServiceJoined;
        public event TypedEventHandler<IProvider, ServiceDroppedEventArgs> ServiceDropped;
    }
}
