using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceProviders;
using Newtonsoft.Json;
using OpenAlljoynExplorer.Support;
using Shared.Support;
using Windows.Storage;
using Windows.UI.Popups;

namespace OpenAlljoynExplorer.Models
{
    public class Favorite : Bindable, IEquatable<Favorite>
    {
        public string DeviceId { get; set; }
        public string ObjectPath { get; set; }
        public string InterfaceName { get; set; }
        public string MethodName { get; set; }

        public bool IsAvailable
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public IService Service { get; private set; }
        public IInterface Interface { get; private set; }
        public IMethod Method { get; private set; }

        public bool Equals(Favorite other)
        {
            return GetHashCode() == other.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Favorite);
        }
        public override int GetHashCode()
        {
            //return Shared.Support.StringHelper.CreateMD5(DeviceId+ ObjectPath+ InterfaceName+ MethodName);
            return (DeviceId + ObjectPath + InterfaceName + MethodName).GetHashCode();
        }


        internal static bool Remove(Favorite favorite)
        {
            List<Favorite> favorites = GetAll();
            var ret = favorites.Remove(favorite);
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["Favorites"] = JsonConvert.SerializeObject(favorites);
            return ret;
        }

        internal static bool Add(Favorite favorite)
        {
            var localSettings = ApplicationData.Current.LocalSettings;

            List<Favorite> favorites = GetAll();

            if (favorites.Contains(favorite))
            {
                return false;
            }
            else
            {
                favorites.Add(favorite);
                localSettings.Values["Favorites"] = JsonConvert.SerializeObject(favorites);
                return true;
            }
        }

        internal static List<Favorite> GetAll()
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
            return favorites;
        }

        internal static async Task SetAvailableFavorite(IEnumerable<Favorite> favorites , AllJoynService availableService)
        {
            foreach (var favorite in favorites)
            {
                if (availableService.Service.AboutData.DeviceId == favorite.DeviceId)
                {
                    var navigationObject = availableService?.Service?.Objects?.FirstOrDefault(o => o != null & o.Path == favorite.ObjectPath);
                    if (navigationObject?.Interfaces != null)
                    {
                        var navigationInterface = navigationObject.Interfaces.FirstOrDefault(i => i.Name == favorite.InterfaceName);
                        if (navigationInterface != null)
                        {
                            if (favorite.MethodName != null)
                            {
                                var method = navigationInterface.GetMethod(favorite.MethodName);
                                if (method != null)
                                {
                                    // favorite method is available!
                                    await Dispatcher.Dispatch(() =>
                                    {
                                        favorite.IsAvailable = true;
                                        favorite.Service = availableService.Service;
                                        favorite.Interface = navigationInterface;
                                        favorite.Method = method;
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        [Obsolete("Use properties Service, Interface, Method of Favorite set by SetAvailableFavorite instead")]
        internal static MethodModel GetFavoriteModel(AllJoynService service)
        {
            List<Favorite> favorites = GetAll();

            foreach (var favorite in favorites)
            {
                if (service.Service.AboutData.DeviceId == favorite.DeviceId)
                {
                    var navigationObject = service.Service.Objects.FirstOrDefault(o => o.Path == favorite.ObjectPath);
                    if (navigationObject?.Interfaces != null)
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
