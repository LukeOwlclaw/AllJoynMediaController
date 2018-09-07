using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;

namespace OpenAlljoynExplorer.Models
{
    class MyInterface: IInterface
    {
        public MyInterface(IInterface i)
        {
            Name = i.Name;
            IntrospectXml = i.IntrospectXml;
        }

        public IProperty GetProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        public IMethod GetMethod(string methodName)
        {
            throw new NotImplementedException();
        }

        public ISignal GetSignal(string SignalName)
        {
            throw new NotImplementedException();
        }

        public IBusObject BusObject => throw new NotImplementedException();

        public string Name { get; }

        public string IntrospectXml { get; }

        public IReadOnlyDictionary<string, string> Annotations => throw new NotImplementedException();

        public IReadOnlyList<ISignal> Signals => throw new NotImplementedException();

        public IReadOnlyList<IMethod> Methods => throw new NotImplementedException();

        public IReadOnlyList<IProperty> Properties => throw new NotImplementedException();
    }
}
