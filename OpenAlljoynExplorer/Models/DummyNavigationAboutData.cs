using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Windows.Foundation;

namespace OpenAlljoynExplorer.Models
{
    class DummyNavigationAboutData : IAboutData
    {
        public IReadOnlyList<KeyValuePair<string, AllJoynMessageArgVariant>> GetAllFields()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IAboutIcon> GetIconAsync()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<KeyValuePair<string, AllJoynMessageArgVariant>> AnnouncedFields => throw new NotImplementedException();

        public IReadOnlyList<string> SupportedLanguages => throw new NotImplementedException();

        public string SupportUrl => throw new NotImplementedException();

        public string HardwareVersion => throw new NotImplementedException();

        public string AllJoynSoftwareVersion => throw new NotImplementedException();

        public string SoftwareVersion => throw new NotImplementedException();

        public string DateOfManufacture => throw new NotImplementedException();

        public string Description => throw new NotImplementedException();

        public string ModelNumber => throw new NotImplementedException();

        public string Manufacturer => "Will only navigation to requested device when (if) it is found.";

        public string AppName => "No services will be shown.";

        public string DeviceId => throw new NotImplementedException();

        public string DeviceName => "Navigation active";

        public string DefaultLanguage => throw new NotImplementedException();

        public string AppId => throw new NotImplementedException();

        public string CurrentLanguage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
