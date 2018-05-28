using System;
using System.Collections.Generic;
using System.Linq;
using DeviceProviders;

namespace MediaControl.Models
{
    /// <summary>
    /// A media playlist
    /// </summary>
    public class Playlist
    {
        /// <summary>
        /// Creates managed instance of playlist.
        /// </summary>
        /// <param name="result">Result of call to GetPlaylist</param>
        internal Playlist(IList<object> result)
        {
            int index = 0;
            try
            {
                var items = result[0] as IList<object>;
                if (items == null)
                {
                    var errorString = result[0] as String;
                    if (errorString != null)
                    {
                        throw new ArgumentException("Received playlist contains error: " + errorString);
                    }
                }
                ControllerType = result[1] as string;
                PlaylistUserData = result[2] as string;
                Items = items.Select(i => new Media(i as AllJoynMessageArgStructure));

                foreach (var item in Items)
                {
                    var current = item;
                    index++;
                }
                var list = Items.ToList();

            }
            catch (Exception ex)
            {
                ex = ex;
            }
        }

        /// <summary>
        /// Gets the items in the playlist.
        /// </summary>
        public IEnumerable<Media> Items { get; }

        /// <summary>
        /// Gets a user-defined string to identify the controller type.
        /// </summary>
        public string ControllerType { get; }

        /// <summary>
        /// Gets the user-defined data
        /// </summary>
        public string PlaylistUserData { get; }

        internal object ListToParameter()
        {
            var list = new List<object>(Items.Select(i => i.ToParameter()));
            return list.ToArray();            
        }
    }
}