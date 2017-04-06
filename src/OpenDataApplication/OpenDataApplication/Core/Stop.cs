namespace OpenDataApplication.Core
{
    using GMap.NET;
    using System.Diagnostics;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a RET station (tram, bus, metro and boat)
    /// </summary>
    [DebuggerDisplay("{Name} at {Position}")]
    public sealed class Stop : ISerializable
    {
        /// <summary>
        /// The name of the station.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// A description of the station.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// The position in latitude and longitude.
        /// </summary>
        public PointLatLng Position { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stop"/> class with default values.
        /// </summary>
        public Stop()
        {
            Name = string.Empty;
            Description = string.Empty;
            Position = new PointLatLng();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stop"/> class from deserialized information.
        /// </summary>
        /// <param name="info"> The information from the deserialization process. </param>
        /// <param name="context"> The context of creating. </param>
        public Stop(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("name");
            Description = info.GetString("desc");
            Position = new PointLatLng(info.GetDouble("latitude"), info.GetDouble("longitude"));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("desc", Description);
            info.AddValue("latitude", Position.Lat);
            info.AddValue("longitude", Position.Lng);
        }
    }
}