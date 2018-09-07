using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.Models
{
    public class MyBusObject : IBusObject
    {
        
        public MyBusObject(IBusObject busObject)
        {
            Path = busObject.Path;
            if (busObject.Interfaces == null)
            {
                this.Interfaces = null;
                return;
            }

            this.Interfaces = new List<IInterface>();
            
            foreach (var i in busObject.Interfaces)
            {
                Interfaces.Add(new MyInterface(i));
            }
        }

        public IInterface GetInterface(string interfaceName)
        {
            throw new NotImplementedException();
        }

        public IBusObject GetChild(string fullPath)
        {
            throw new NotImplementedException();
        }

        public IService Service => throw new NotImplementedException();

        public string Path { get; }

        public IList<IBusObject> ChildObjects => throw new NotImplementedException();

        public IList<IInterface> Interfaces { get; }
    }
}
