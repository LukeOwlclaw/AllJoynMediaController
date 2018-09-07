using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Windows.Foundation;

namespace OpenAlljoynExplorer.Models
{
    class MyProperty: IProperty
    {
        public IAsyncOperation<ReadValueResult> ReadValueAsync()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<AllJoynStatus> SetValueAsync(object newValue)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<string, string> Annotations => throw new NotImplementedException();

        public IInterface Interface => throw new NotImplementedException();

        public bool CanWrite => throw new NotImplementedException();

        public bool CanRead => throw new NotImplementedException();

        public ITypeDefinition TypeInfo => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public event TypedEventHandler<IProperty, object> ValueChanged;
    }
}
