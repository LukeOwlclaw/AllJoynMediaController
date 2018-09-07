using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Windows.Foundation;

namespace OpenAlljoynExplorer.Models
{
    class MySignal: ISignal
    {
        public IReadOnlyDictionary<string, string> Annotations => throw new NotImplementedException();

        public IInterface Interface => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public IList<ParameterInfo> Signature => throw new NotImplementedException();

        public event TypedEventHandler<ISignal, IList<object>> SignalRaised;
    }
}
