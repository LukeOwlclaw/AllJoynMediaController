using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;

namespace OpenAlljoynExplorer.Models
{
    public class Favorite
    {
        public string DeviceId { get; set; }
        public string ObjectPath { get; set; }
        public string InterfaceName { get; set; }
        public string MethodName { get; set; }

        internal static void Add(Favorite favorite)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            List<Favorite> favorites;
            try
            {
                favorites = JsonConvert.DeserializeObject<List<Favorite>>(localSettings.Values["Favorites"].ToString());
            }
            catch
            {
                favorites = new List<Favorite>();
            }
            favorites.Add(favorite);
            localSettings.Values["Favorites"] = JsonConvert.SerializeObject(favorites);
        }

        internal static MethodModel GetFavoriteModel(AllJoynService service)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            List<Favorite> favorites = null;
            try
            {
                favorites = JsonConvert.DeserializeObject<List<Favorite>>(localSettings.Values["Favorites"].ToString());
            }
            catch
            { }

            if (favorites == null)
                return null;

            foreach (var favorite in favorites)
            {
                if (service.Service.AboutData.DeviceId == favorite.DeviceId)
                {
                    var navigationObject = service.Service.Objects.FirstOrDefault(o => o.Path == favorite.ObjectPath);
                    if (navigationObject != null)
                    {
                        var navigationInterface = navigationObject.Interfaces.FirstOrDefault(i => i.Name == favorite.InterfaceName);
                        if (navigationInterface != null)
                        {
                            if (favorite.MethodName != null)
                            {
                                var method = navigationInterface.GetMethod(favorite.MethodName);
                                if (method != null)
                                {
                                    var model = new MethodModel
                                    {
                                        Service = service.Service,
                                        Interface = navigationInterface,
                                        Method = method
                                    };
                                    return model;
                                }
                            }
                        }
                    }
                }
            }
            return null;

        }
    }
}
