﻿using System;
using System.Collections.Generic;
using System.Linq;
using DeviceProviders;
using System.Collections.ObjectModel;

namespace ControlApp.Models
{
    /// <summary>
    /// A media entry item
    /// </summary>
    public class Media
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Media"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="title">The title.</param>
        /// <param name="artist">The artist.</param>
        /// <param name="thumbnailUrl">The thumbnail URL.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="mediaType">Type of the media.</param>
        /// <param name="album">The album.</param>
        /// <param name="genre">The genre.</param>
        public Media(string url, string title = null, string artist = null,
            Uri thumbnailUrl = null, TimeSpan? duration = null,
            string mediaType = null, string album = null, string genre = null)
        {
            Url = url;
            Title = title;
            Artist = artist;
            ThumbnailUrl = thumbnailUrl;
            Duration = duration == null ? TimeSpan.Zero : duration.Value;
            MediaType = mediaType;
            Album = album;
            Genre = genre;
        }

        internal Media(AllJoynMessageArgStructure s)
        {
            // "(ssssxsssa{ss}a{sv}v)
            Url = s[0] as string;
            Title = s[1] as string;
            Artist = s[2] as string;
            ThumbnailUrl = s[3] is string ? new Uri(s[3] as string) : null;
            Duration = TimeSpan.FromMilliseconds((long)s[4]);
            MediaType = s[5] as string;
            Album = s[6] as string;
            Genre = s[7] as string;
            var otherDataArg = s[8] as IList<KeyValuePair<object, object>>;
            OtherData = new Dictionary<string, string>();
            foreach (var item in otherDataArg)
            {
                OtherData.Add((string)item.Key, (string)item.Value);
            }

            var mediumArg = s[9] as IList<KeyValuePair<object, object>>;
            MediumDesc = new Dictionary<string, object>();
            foreach (var item in mediumArg)
            {
                MediumDesc.Add((string)item.Key, item.Value);
            }

            UserData = s[10] as AllJoynMessageArgVariant;
        }

        internal IList<Variant> ToParameter2()
        {
            var ret = new List<Variant>();
            var v = new Variant();            
            return ret;
        }

            internal IList<object> ToParameter()
        {
            var t = AllJoynTypeDefinition.CreateTypeDefintions("(ssssxsssa{ss}a{sv}v)").First();

            // ITypeDefinition.h -->
            // public enum class TypeId
            //    {
            //        Invalid = 0,
            //    Boolean = 'b',                               // maps to ALLJOYN_BOOLEAN
            //    Double = 'd',                                // maps to ALLJOYN_DOUBLE
            //    Dictionary = 'e',                            // maps to an array of ALLJOYN_DICT_ENTRY: a{**}
            //    Signature = 'g',                             // maps to ALLJOYN_SIGNATURE (string)
            //    Int32 = 'i',                                 // maps to ALLJOYN_INT32
            //    Int16 = 'n',                                 // maps to ALLJOYN_INT16
            //    ObjectPath = 'o',                            // maps to ALLJOYN_OBJECT_PATH (string)
            //    Uint16 = 'q',                                // maps to ALLLJOYN_UINT16
            //    Struct = 'r',                                // maps to ALLJOYN_STRUCT
            //    String = 's',                                // maps to ALLJOYN_STRING
            //    Uint64 = 't',                                // maps to ALLJOYN_UINT64
            //    Uint32 = 'u',                                // maps to ALLJOYN_UINT32
            //    Variant = 'v',                               // maps to ALLJOYN_VARIANT
            //    Int64 = 'x',                                 // maps to ALLJOYN_INT64
            //    Uint8 = 'y',                                 // maps to ALLJOYN_BYTE
            //    ArrayByte = 'a',
            //    ArrayByteMask = 0xFF,
            //    BooleanArray    = ('b' << 8) | ArrayByte,    // maps to ALLJOYN_BOOLEAN_ARRAY
            //    DoubleArray     = ('d' << 8) | ArrayByte,    // maps to ALLJOYN_DOUBLE_ARRAY
            //    Int32Array      = ('i' << 8) | ArrayByte,    // maps to ALLJOYN_INT32_ARRAY
            //    Int16Array      = ('n' << 8) | ArrayByte,    // maps to ALLJOYN_INT16_ARRAY
            //    Uint16Array     = ('q' << 8) | ArrayByte,    // maps to ALLJOYN_UINT16_ARRAY
            //    Uint64Array     = ('t' << 8) | ArrayByte,    // maps to ALLJOYN_UINT64_ARRAY
            //    Uint32Array     = ('u' << 8) | ArrayByte,    // maps to ALLJOYN_UINT32_ARRAY
            //    VariantArray    = ('v' << 8) | ArrayByte,    // no AllJoyn typeid equivalent defined
            //    Int64Array      = ('x' << 8) | ArrayByte,    // maps to ALLJOYN_INT64_ARRAY
            //    Uint8Array      = ('y' << 8) | ArrayByte,    // maps to ALLJOYN_BYTE_ARRAY
            //    SignatureArray  = ('g' << 8) | ArrayByte,    // no AllJoyn typeid equivalent defined
            //    ObjectPathArray = ('o' << 8) | ArrayByte,    // no AllJoyn typeid equivalent defined
            //    StringArray     = ('s' << 8) | ArrayByte,    // no AllJoyn typeid equivalent defined
            //    StructArray     = ('r' << 8) | ArrayByte,    // no AllJoyn typeid equivalent defined
            // };
            var argument = new AllJoynMessageArgStructure(t);

            // var count = paramDef.Fields.Count;
            // string[] types = paramDef.Fields.Select(f => f.Type.ToString()).ToArray();
            // List<object> argument = new List<object>();
            argument.Add(Url);
            argument.Add(Title ?? " ");
            argument.Add(Artist ?? " ");
            argument.Add(ThumbnailUrl?.OriginalString ?? " ");
            argument.Add((long)Duration.TotalMilliseconds);
            argument.Add(MediaType ?? " ");
            argument.Add(Album ?? " ");
            argument.Add(Genre ?? " ");

            // Other data: a{ss}
            var otherData = new Dictionary<object, object>();
            if (OtherData != null)
            {
                foreach (var item in OtherData)
                {
                    otherData.Add(item.Key, item.Value);
                }
            }

            argument.Add(otherData.ToList());

            // medium desc: a{sv}
            var mediumDesc = new Dictionary<object, object>();
            if (OtherData != null)
            {
                foreach (var item in OtherData)
                {
                    mediumDesc.Add(item.Key, item.Value);
                }
            }
            
            argument.Add(mediumDesc.ToList());

            // AllJoynMessageArgVariant v = new AllJoynMessageArgVariant();
            // var arg = new DeviceProviders.AllJoynMessageArgVariant(AllJoynTypeDefinition.CreateTypeDefintions("v").First(), 0);
            // arg.Value = "upnp";
            argument.Add(UserData ?? "upnp"); // Variant: userdata
            return argument;
        }

        /// <summary>
        /// Gets the url to the item
        /// </summary>
        public string Url { get; }

        /// <summary>
        /// Gets the title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the channel name if this media is a stream/channel and has a name.
        /// </summary>
        public string Channel
        {
            get
            {
                OtherData.TryGetValue("channel", out string channel);
                return channel;
            }
        }

        /// <summary>
        /// Gets the artist
        /// </summary>
        public string Artist { get; }

        /// <summary>
        /// Gets the media thumbnail url
        /// </summary>
        public Uri ThumbnailUrl { get; }

        /// <summary>
        /// Gets the duration
        /// </summary>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets the type of media
        /// </summary>
        public string MediaType { get; }

        /// <summary>
        /// Gets the album
        /// </summary>
        public string Album { get; }

        /// <summary>
        /// Gets the genre
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        /// <value>The additional data.</value>
        public IDictionary<string, string> OtherData { get; set; }

        /// <summary>
        /// Gets or sets additional media descriptors.
        /// </summary>
        /// <value>The medium desc.</value>
        public IDictionary<string, object> MediumDesc { get; set; }

        private object UserData { get; set; }

        public enum AvailableMediaClass
        {
            Song,
            Channel
        }

        public AvailableMediaClass MediaClass
        {
            get
            {
                if (Url.EndsWith(".mp3"))
                    return AvailableMediaClass.Song;
                if (Duration == TimeSpan.Zero)
                    return AvailableMediaClass.Channel;
                return AvailableMediaClass.Song;
            }
        }
    }
}
