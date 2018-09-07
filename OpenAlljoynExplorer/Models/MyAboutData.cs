using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Windows.Foundation;

namespace OpenAlljoynExplorer.Models
{
    class MyAboutData : IAboutData
    {
        
        public MyAboutData(IAboutData aboutData)
        {
            this.AllJoynSoftwareVersion = aboutData.AllJoynSoftwareVersion;
            this.AnnouncedFields = aboutData.AnnouncedFields;
            this.AppId = aboutData.AppId;
            this.AppName = aboutData.AppName;
            this.CurrentLanguage = aboutData.CurrentLanguage;
            this.DefaultLanguage = aboutData.DefaultLanguage;
            this.DeviceId = aboutData.DeviceId;
            this.DeviceName = aboutData.DeviceName;
            this.Manufacturer = aboutData.Manufacturer;
            this.ModelNumber = aboutData.ModelNumber;
            this.SupportUrl = aboutData.SupportUrl;
            this.HardwareVersion = aboutData.HardwareVersion;
            this.AllJoynSoftwareVersion = aboutData.AllJoynSoftwareVersion;
            this.DateOfManufacture = aboutData.DateOfManufacture;
            this.SoftwareVersion = aboutData.SoftwareVersion;
            this.Description = aboutData.Description;
            this.SupportedLanguages = aboutData.SupportedLanguages;
        }

        public IReadOnlyList<KeyValuePair<string, AllJoynMessageArgVariant>> GetAllFields()
        {
            throw new NotImplementedException();
        }

        public IAsyncOperation<IAboutIcon> GetIconAsync()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<KeyValuePair<string, AllJoynMessageArgVariant>> AnnouncedFields { get; }

        public IReadOnlyList<string> SupportedLanguages { get; }

        public string SupportUrl { get; }

        public string HardwareVersion { get; }

        public string AllJoynSoftwareVersion { get; }

        public string SoftwareVersion { get; }

        public string DateOfManufacture { get; }

        public string Description { get; }

        public string ModelNumber { get; }

        public string Manufacturer { get; }

        public string AppName { get; }

        public string DeviceId { get; }

        public string DeviceName { get; }

        public string DefaultLanguage { get; }

        public string AppId { get; }

        public string CurrentLanguage { get; set; }
    }
}
