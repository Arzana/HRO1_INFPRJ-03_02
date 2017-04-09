namespace OpenDataApplication.Core.DataTypes
{
    using GMap.NET;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a NS Station
    /// </summary>
    [DebuggerDisplay("{FullName}({UIC}) at {Position}")]
    public sealed class Station : ISerializable
    {
        /// <summary>
        /// Internal dutch station identity.
        /// </summary>
        public short Id { get; private set; }
        /// <summary>
        /// Human readable internal dutch station identity.
        /// </summary>
        public string Code { get; private set; }
        /// <summary>
        /// International station identity.
        /// </summary>
        public int UIC { get; private set; }
        /// <summary>
        /// The stations full name.
        /// </summary>
        public string FullName { get; private set; }
        /// <summary>
        /// A slightly shorter version of the stations full name.
        /// </summary>
        public string MiddleName { get; private set; }
        /// <summary>
        /// A short version of the stations name.
        /// </summary>
        public string ShortName { get; private set; }
        /// <summary>
        /// A easy readable version of the stations name.
        /// </summary>
        public string FriendlyName { get; private set; }
        /// <summary>
        /// A full upper country code.
        /// </summary>
        public string CountryCode { get; private set; }
        /// <summary>
        /// The type of station.
        /// </summary>
        public StationType Type { get; private set; }
        /// <summary>
        /// The position in latitude and longitude.
        /// </summary>
        public PointLatLng Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Station"/> class with default values.
        /// </summary>
        public Station()
        {
            Id = -1;
            Code = string.Empty;
            UIC = -1;
            FullName = string.Empty;
            MiddleName = string.Empty;
            ShortName = string.Empty;
            FriendlyName = string.Empty;
            CountryCode = string.Empty;
            Type = StationType.none;
            Position = new PointLatLng();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Station"/> class from deserialized information.
        /// </summary>
        /// <param name="info"> The information from the deserialization process. </param>
        /// <param name="context"> The context of creating. </param>
        public Station(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt16("id");
            Code = info.GetString("code");
            UIC = info.GetInt32("uic");
            FullName = info.GetString("naam");
            MiddleName = info.GetString("middel_naam");
            ShortName = info.GetString("korte_naam");
            FriendlyName = info.GetString("friendly");
            CountryCode = info.GetString("land");
            Type = (StationType)info.GetValue("type", typeof(StationType));
            Position = new PointLatLng(info.GetDouble("geo_lat"), info.GetDouble("geo_lng"));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("id", Id);
            info.AddValue("code", Code);
            info.AddValue("uic", UIC);
            info.AddValue("naam", FullName);
            info.AddValue("middel_naam", MiddleName);
            info.AddValue("korte_naam", ShortName);
            info.AddValue("friendly", FriendlyName);
            info.AddValue("land", CountryCode);
            info.AddValue("type", Type);
            info.AddValue("geo_lat", Position.Lat);
            info.AddValue("geo_lng", Position.Lng);
        }
    }
}